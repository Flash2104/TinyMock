
import core from 'comindware/core';
import EditorCanvasView from 'demoPage/views/EditorCanvasView';

export default function() {
    const model = new Backbone.Model({
        referenceValue: {
            id: 'test.1',
            text: 'Test Reference 1'
        }
    });

    return new EditorCanvasView({
        editor: new core.form.editors.ReferenceEditor({
            model,
            key: 'referenceValue',
            autocommit: true,
            showEditButton: true,
            controller: new core.form.editors.reference.controllers.DemoReferenceEditorController()
        }),
        presentation: "{{#if referenceValue}}{ id: '{{referenceValue.id}}', text: '{{referenceValue.text}}' }{{else}}null{{/if}}"
    });
}
