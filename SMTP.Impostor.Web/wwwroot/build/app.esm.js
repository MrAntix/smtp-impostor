import { p as promiseResolve, b as bootstrapLazy } from './p-2c960d8c.js';
export { s as setNonce } from './p-2c960d8c.js';
import { g as globalScripts } from './p-57154ccb.js';
import './p-74950a4f.js';

/*
 Stencil Client Patch Browser v3.2.1 | MIT Licensed | https://stenciljs.com
 */
const patchBrowser = () => {
    const importMeta = import.meta.url;
    const opts = {};
    // TODO(STENCIL-663): Remove code related to deprecated `safari10` field.
    if (importMeta !== '') {
        opts.resourcesUrl = new URL('.', importMeta).href;
        // TODO(STENCIL-661): Remove code related to the dynamic import shim
        // TODO(STENCIL-663): Remove code related to deprecated `safari10` field.
    }
    return promiseResolve(opts);
};

patchBrowser().then(options => {
  globalScripts();
  return bootstrapLazy([["p-bdf677fc",[[1,"app-root",{"state":[32]}],[1,"smtp-host",{"value":[16],"showMessages":[516,"show-messages"],"showConfiguration":[516,"show-configuration"],"toggleState":[64],"searchMessages":[64]}],[1,"impostor-hub",{"socketProvider":[16],"url":[1],"status":[1026],"connectAsync":[64],"sendAsync":[64],"disconnectAsync":[64]}],[17,"smtp-host-configuration",{"value":[16]}],[1,"app-input",{"placeholder":[513],"clearButton":[516,"clear-button"],"iconType":[513,"icon-type"],"value":[1]}],[1,"app-popup",{"isOpen":[1540,"is-open"],"modal":[4],"position":[1537],"shift":[1537],"toggle":[64]}],[1,"app-icon",{"type":[1],"rotate":[514],"flipHorizontal":[516,"flip-horizontal"],"flipVertical":[516,"flip-vertical"],"scale":[514]}]]]], options);
});

//# sourceMappingURL=app.esm.js.map