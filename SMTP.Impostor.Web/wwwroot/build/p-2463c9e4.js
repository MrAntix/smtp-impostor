import { C as CSS, p as plt, w as win, a as promiseResolve, d as doc, N as NAMESPACE, c as Context } from './p-bbb3d3bd.js';
import { L as LogLevel, c as consoleLogMethodProvider } from './p-df055c62.js';

/*
 Stencil Client Patch v1.16.0 | MIT Licensed | https://stenciljs.com
 */
const noop = () => {
    /* noop*/
};
const IS_DENO_ENV = typeof Deno !== 'undefined';
const IS_NODE_ENV = !IS_DENO_ENV &&
    typeof global !== 'undefined' &&
    typeof require === 'function' &&
    !!global.process &&
    typeof __filename === 'string' &&
    (!global.origin || typeof global.origin !== 'string');
const IS_DENO_WINDOWS_ENV = IS_DENO_ENV && Deno.build.os === 'windows';
const getCurrentDirectory = IS_NODE_ENV ? process.cwd : IS_DENO_ENV ? Deno.cwd : () => '/';
const exit = IS_NODE_ENV ? process.exit : IS_DENO_ENV ? Deno.exit : noop;
const getDynamicImportFunction = (namespace) => `__sc_import_${namespace.replace(/\s|-/g, '_')}`;
const patchEsm = () => {
    // NOTE!! This fn cannot use async/await!
    // @ts-ignore
    if ( !(CSS && CSS.supports && CSS.supports('color', 'var(--c)'))) {
        // @ts-ignore
        return __sc_import_app(/* webpackChunkName: "polyfills-css-shim" */ './p-514b3755.js').then(() => {
            if ((plt.$cssShim$ = win.__cssshim)) {
                return plt.$cssShim$.i();
            }
            else {
                // for better minification
                return 0;
            }
        });
    }
    return promiseResolve();
};
const patchBrowser = () => {
    {
        // shim css vars
        plt.$cssShim$ = win.__cssshim;
    }
    // @ts-ignore
    const scriptElm =  Array.from(doc.querySelectorAll('script')).find(s => new RegExp(`\/${NAMESPACE}(\\.esm)?\\.js($|\\?|#)`).test(s.src) || s.getAttribute('data-stencil-namespace') === NAMESPACE)
        ;
    const opts =  scriptElm['data-opts'] || {} ;
    if ( 'onbeforeload' in scriptElm && !history.scrollRestoration /* IS_ESM_BUILD */) {
        // Safari < v11 support: This IF is true if it's Safari below v11.
        // This fn cannot use async/await since Safari didn't support it until v11,
        // however, Safari 10 did support modules. Safari 10 also didn't support "nomodule",
        // so both the ESM file and nomodule file would get downloaded. Only Safari
        // has 'onbeforeload' in the script, and "history.scrollRestoration" was added
        // to Safari in v11. Return a noop then() so the async/await ESM code doesn't continue.
        // IS_ESM_BUILD is replaced at build time so this check doesn't happen in systemjs builds.
        return {
            then() {
                /* promise noop */
            },
        };
    }
    {
        opts.resourcesUrl = new URL('.', new URL(scriptElm.getAttribute('data-resources-url') || scriptElm.src, win.location.href)).href;
        {
            patchDynamicImport(opts.resourcesUrl, scriptElm);
        }
        if ( !win.customElements) {
            // module support, but no custom elements support (Old Edge)
            // @ts-ignore
            return __sc_import_app(/* webpackChunkName: "polyfills-dom" */ './p-c95129be.js').then(() => opts);
        }
    }
    return promiseResolve(opts);
};
const patchDynamicImport = (base, orgScriptElm) => {
    const importFunctionName = getDynamicImportFunction(NAMESPACE);
    try {
        // test if this browser supports dynamic imports
        // There is a caching issue in V8, that breaks using import() in Function
        // By generating a random string, we can workaround it
        // Check https://bugs.chromium.org/p/chromium/issues/detail?id=990810 for more info
        win[importFunctionName] = new Function('w', `return import(w);//${Math.random()}`);
    }
    catch (e) {
        // this shim is specifically for browsers that do support "esm" imports
        // however, they do NOT support "dynamic" imports
        // basically this code is for old Edge, v18 and below
        const moduleMap = new Map();
        win[importFunctionName] = (src) => {
            const url = new URL(src, base).href;
            let mod = moduleMap.get(url);
            if (!mod) {
                const script = doc.createElement('script');
                script.type = 'module';
                script.crossOrigin = orgScriptElm.crossOrigin;
                script.src = URL.createObjectURL(new Blob([`import * as m from '${url}'; window.${importFunctionName}.m = m;`], { type: 'application/javascript' }));
                mod = new Promise(resolve => {
                    script.onload = () => {
                        resolve(win[importFunctionName].m);
                        script.remove();
                    };
                });
                moduleMap.set(url, mod);
                doc.head.appendChild(script);
            }
            return mod;
        };
    }
};

Context.store = (() => {
    let _store;
    const setStore = (store) => {
        _store = store;
    };
    const getState = () => {
        return _store && _store.getState();
    };
    const getStore = () => {
        return _store;
    };
    const mapDispatchToProps = (component, props) => {
        Object.keys(props).forEach(actionName => {
            const action = props[actionName];
            Object.defineProperty(component, actionName, {
                get: () => (...args) => _store.dispatch(action(...args)),
                configurable: true,
                enumerable: true,
            });
        });
    };
    const mapStateToProps = (component, mapState) => {
        const _mapStateToProps = (_component, _mapState) => {
            const mergeProps = mapState(_store.getState());
            Object.keys(mergeProps).forEach(newPropName => {
                const newPropValue = mergeProps[newPropName];
                component[newPropName] = newPropValue;
            });
        };
        const unsubscribe = _store.subscribe(() => _mapStateToProps());
        _mapStateToProps();
        return unsubscribe;
    };
    return {
        getStore,
        setStore,
        getState,
        mapDispatchToProps,
        mapStateToProps,
    };
})();

const appGlobalScript = async () => {
    globalThis.logScope = { default: LogLevel.debug };
    globalThis.loggerMethodProvider = consoleLogMethodProvider;
    globalThis.getLogger = (scope) => {
        const getMethod = (methodLevel) => {
            const method = globalThis.loggerMethodProvider(methodLevel);
            return (...args) => {
                const scopeLevel = Reflect.has(globalThis.logScope, scope)
                    ? globalThis.logScope[scope]
                    : globalThis.logScope.default;
                if (methodLevel >= scopeLevel)
                    method([`[${LogLevel[methodLevel]}] ${scope}`, ...args]);
            };
        };
        return {
            debug: getMethod(LogLevel.debug),
            info: getMethod(LogLevel.info),
            warn: getMethod(LogLevel.warn),
            error: getMethod(LogLevel.error)
        };
    };
};

const globalScripts = () => {
  appGlobalScript();
};

export { patchEsm as a, globalScripts as g, patchBrowser as p };