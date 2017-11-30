
import core from 'comindware/core';

export default function(){
    let recorder;
    const model = new Backbone.Model({
        text: '',
        isMockupGenerate: false,
        isRecordStart: false,
        view: ''
    });

    const formSchema = {
        text: {
            title: 'paste you markup text here',
            autocommit: true,
            type: 'TextArea'
        }
    };

    const sendData = () => {
        Ajax.Mock.Execute(model.get('text'))
            .then(data => loadData(data));
    };

    const loadData = data => {
        const mapedData = mapData(data);
        console.log('mapedData', mapedData);
        const view = new core.View({
            model: mapedData.model,
            schema: [{
                type: 'v-container',
                items: mapedData.schema
            }]
        });
        console.log('view', view);
        model.set('view', view);
        model.set('isMockupGenerate', true);
        model.set('text', '');
    };

    const startRecord = () => {
        console.log('startRecord');
        model.set('isRecordStart', true);
        recorder.start();
    };

    const stopRecord = () => {
        console.log('stopRecord');
        model.set('isRecordStart', false);
        recorder.stop();
    };

    const onRecordingReady = e => {
        console.log('onRecordingReady');
        const audio = document.getElementById('audio');
        // e.data contains a blob representing the recording
        const audioSrc = URL.createObjectURL(e.data);
        Ajax.Mock.ExecuteVoice(audioSrc)
            .then( data => loadData(data));
    };

    const mapData = (data) => {
        const schema = [];
        const modelObj = {};
        _.each(data, dataItem => {
            schema.push({
                type: dataItem.scheme.type,
                key: dataItem.id,
                title: dataItem.scheme.title
            });
            modelObj[dataItem.id] = dataItem.value;
        });

        return {
            schema,
            model: new Backbone.Model(modelObj)
        }
    };

    const view = new core.layout.Form({
        model,
        schema: formSchema,
        content: new core.layout.VerticalLayout({
            rows: [
                core.layout.createFieldAnchor('text'),
                new core.layout.HorizontalLayout({
                    columns: [
                        new core.layout.Button({
                            text: 'Create you markup',
                            handler: () => {
                                sendData();
                            }
                        }),
                        new core.layout.Button({
                            text: 'Record voice',
                            handler: () => {
                                if (model.get('isRecordStart')) {
                                    stopRecord();
                                } else {
                                    startRecord();
                                }
                            }
                        })
                    ]
                })
            ]
        })
    });
    sendData();

    navigator.mediaDevices.getUserMedia({
        audio: true
    }).then(function (stream) {
        recorder = new MediaRecorder(stream);

        // listen to dataavailable, which gets triggered whenever we have
        // an audio blob available
        recorder.addEventListener('dataavailable', onRecordingReady);
    });

    return view;
}
