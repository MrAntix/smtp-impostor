import { B as BUILD, c as consoleDevInfo, H, d as doc, N as NAMESPACE, p as promiseResolve, b as bootstrapLazy } from './index-f0ebbfcb.js';
export { s as setNonce } from './index-f0ebbfcb.js';
import { g as globalScripts } from './app-globals-79dcb1fe.js';
import './app-d455fece.js';

/*
 Stencil Client Patch Browser v4.7.0 | MIT Licensed | https://stenciljs.com
 */
const patchBrowser = () => {
    // NOTE!! This fn cannot use async/await!
    if (BUILD.isDev && !BUILD.isTesting) {
        consoleDevInfo('Running in development mode.');
    }
    if (BUILD.cloneNodeFix) {
        // opted-in to polyfill cloneNode() for slot polyfilled components
        patchCloneNodeFix(H.prototype);
    }
    const scriptElm = BUILD.scriptDataOpts
        ? Array.from(doc.querySelectorAll('script')).find((s) => new RegExp(`\/${NAMESPACE}(\\.esm)?\\.js($|\\?|#)`).test(s.src) ||
            s.getAttribute('data-stencil-namespace') === NAMESPACE)
        : null;
    const importMeta = import.meta.url;
    const opts = BUILD.scriptDataOpts ? (scriptElm || {})['data-opts'] || {} : {};
    if (importMeta !== '') {
        opts.resourcesUrl = new URL('.', importMeta).href;
    }
    return promiseResolve(opts);
};
const patchCloneNodeFix = (HTMLElementPrototype) => {
    const nativeCloneNodeFn = HTMLElementPrototype.cloneNode;
    HTMLElementPrototype.cloneNode = function (deep) {
        if (this.nodeName === 'TEMPLATE') {
            return nativeCloneNodeFn.call(this, deep);
        }
        const clonedNode = nativeCloneNodeFn.call(this, false);
        const srcChildNodes = this.childNodes;
        if (deep) {
            for (let i = 0; i < srcChildNodes.length; i++) {
                // Node.ATTRIBUTE_NODE === 2, and checking because IE11
                if (srcChildNodes[i].nodeType !== 2) {
                    clonedNode.appendChild(srcChildNodes[i].cloneNode(true));
                }
            }
        }
        return clonedNode;
    };
};

patchBrowser().then(options => {
  globalScripts();
  return bootstrapLazy([["smtp-host",[[1,"smtp-host",{"value":[16],"showMessages":[516,"show-messages"],"showConfiguration":[516,"show-configuration"],"toggleState":[64],"searchMessages":[64]}]]],["app-popup",[[1,"app-popup",{"isOpen":[1540,"is-open"],"modal":[4],"position":[1537],"shift":[1537],"toggle":[64]}]]],["impostor-hub",[[1,"impostor-hub",{"socketProvider":[16],"url":[1],"status":[1026],"connectAsync":[64],"sendAsync":[64],"disconnectAsync":[64]}]]],["smtp-host-configuration",[[17,"smtp-host-configuration",{"value":[16]}]]],["app-icon",[[1,"app-icon",{"type":[1],"rotate":[514],"flipHorizontal":[516,"flip-horizontal"],"flipVertical":[516,"flip-vertical"],"scale":[514]}]]],["app-input",[[1,"app-input",{"placeholder":[513],"clearButton":[516,"clear-button"],"iconType":[513,"icon-type"],"value":[1]}]]],["app-root",[[1,"app-root",{"state":[32]}]]]], options);
});

//# sourceMappingURL=app.esm.js.map