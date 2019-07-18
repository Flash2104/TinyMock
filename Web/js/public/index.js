/**
 * Developer: Stepan Burguchev
 * Date: 2/29/2016
 * Copyright: 2009-2016 Stepan Burguchev®
 *       All Rights Reserved
 * Published under the MIT license
 */

import 'styles/core.css';
import 'styles/demo.css';
import 'lib/prism/prism.css';

import Application from './Application';
import AppRouter from './AppRouter';
import AppController from './AppController';
import * as OfflinePluginRuntime from 'offline-plugin/runtime';
OfflinePluginRuntime.install();
debugger;
Application.appRouter = new AppRouter({
    controller: new AppController()
});

Application.start();
Backbone.history.start();
