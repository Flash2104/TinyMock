/**
 * Developer: Alexander Makarov
 * Date: 13.07.2015
 * Copyright: 2009-2015 Comindware®
 *       All Rights Reserved
 *
 * THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Comindware
 *       The copyright notice above does not evidence any
 *       actual or intended publication of such source code.
 */

const requireCode = require.context('babel-loader!../cases', true);
const requireText = require.context('raw-loader!../cases', true);

import template from 'text-loader!../templates/content.html';
import Prism from 'prism';
import markdown from 'markdown';

export default Marionette.LayoutView.extend({
    modelEvents: {
        change: 'render'
    },

    template: Handlebars.compile(template),

    templateHelpers() {
        return {
            description: markdown.toHTML(this.model.get('description') || '')
        };
    },

    regions: {
        caseRepresentationRegion: '.js-case-representation-region',
        mockupRegion: '.js-mockup-region'
    },

    ui: {
        code: '.js-code'
    },

    onRender() {
        if (!this.model.get('hideCode')) {
            Prism.highlightElement(this.ui.code[0]);
        }
    },

    onShow() {
        let path;
        if (this.model.id) {
            path = `${this.model.get('sectionId')}/${this.model.get('groupId')}/${this.model.id}`;
        } else {
            path = `${this.model.get('sectionId')}/${this.model.get('groupId')}`;
        }

        const code = requireCode(`./${path}`).default;
        const text = requireText(`./${path}`);

        this.model.set('sourceCode', text);
        const representationView = code();
        const modelMockup = representationView.model;
        console.log('representationView', representationView);
        this.listenTo(modelMockup, 'change:isMockupGenerate', () => {
            if (modelMockup.get('isMockupGenerate')) {
                console.log(modelMockup);
                this.mockupRegion.show(modelMockup.get('view'));
                modelMockup.set('isMockupGenerate', false)
            }
        });
        this.caseRepresentationRegion.show(representationView);
    }
});
