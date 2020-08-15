import { r as registerInstance, h, e as createEvent, H as Host, g as getElement, f as getContext } from './p-bbb3d3bd.js';
import { n as newId } from './p-df055c62.js';
import { c as configureStore, d as dispatch, i as initWorkerState, l as loadWorkerState, b as addHost, e as removeHost, s as startHost, f as stopHost, o as openHost, u as updateHost, t as toggleHostConfiguration, g as searchHostMessages, j as deleteHostMessage, k as loadHostMessage, m as startupWorker, n as shutdownWorker, T as Types, D as DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA, H as HubStatus, h as hubSocketProvider, a as hubStatusDisplay, q as hostIsRunning, p as HostStatus, F as Frag } from './p-d99e8c9b.js';

const componentCss = ":host{--host-icon-size:var(--icon-size, .7em);--host-icon-color:var(--icon-color, currentColor);--host-icon-outline-size:var(--icon-outline-size, .0075em);--host-icon-outline-color:var(--host-icon-color, currentColor);display:-ms-inline-flexbox;display:inline-flex;position:relative;height:1em;width:1em;-ms-flex-align:center;align-items:center;-ms-flex-pack:center;justify-content:center}svg{-ms-flex:1 1 auto;flex:1 1 auto;overflow:visible;height:var(--host-icon-size);width:var(--host-icon-size);fill:var(--host-icon-color);stroke-linejoin:round;stroke-linecap:round;stroke:var(--host-icon-outline-color);stroke-width:var(--host-icon-outline-size)}";

const AppIconComponent = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.rotate = 0;
        this.flipHorizontal = false;
        this.flipVertical = false;
        this.scale = 1;
    }
    get(type) {
        switch (type) {
            default:
                return h("text", null, type);
            case "check":
                return h("path", { d: "M5,14 0,9 2,7 5,10 13,2 15,4Z" });
            case "close":
                return (h("path", { d: "M 1,3 3,1 7.5,5.5 12,1 14,3 9.5,7.5 14,12 12,14 7.5,9.5 3,14 1,12 5.5,7.5 Z" }));
            case "cog":
                return [
                    h("path", { d: "M14.991,7.875 A7.5,7.5 0,0,1 14.174,10.921\n                L12.456,10.702 A5.9,5.9 0,0,0 13.393,7.205Z M10.921,14.174 A7.5,7.5 0,0,1 7.875,14.991\n                L7.205,13.393 A5.9,5.9 0,0,0 10.702,12.456Z M3.43,13.8 A7.5,7.5 0,0,1 1.2,11.57\n                L2.249,10.191 A5.9,5.9 0,0,0 4.809,12.751Z M0.009,7.125 A7.5,7.5 0,0,1 0.826,4.079\n                L2.544,4.298 A5.9,5.9 0,0,0 1.607,7.795Z M4.079,0.826 A7.5,7.5 0,0,1 7.125,0.009\n                L7.795,1.607 A5.9,5.9 0,0,0 4.298,2.544Z M11.57,1.2 A7.5,7.5 0,0,1 13.8,3.43\n                L12.751,4.809 A5.9,5.9 0,0,0 10.191,2.249Z" }),
                    h("path", { d: "M7.5,1.5 A6,6 0,0,1 7.5,13.5 M7.5,1.5 A6,6 0,0,0 7.5,13.5 M7.5,10.5 A3,3 0,0,1 7.5,4.5 M7.5,10.5 A3,3 0,0,0 7.5,4.5" })
                ];
            case "delete":
                return (h("path", { d: "M 1,3 3,1 7.5,5.5 12,1 14,3 9.5,7.5 14,12 12,14 7.5,9.5 3,14 1,12 5.5,7.5 Z" }));
            case "paperclip":
                return (h("path", { d: "M3,12A3,3 1,0,0 6,15\n              L9,15A3,3 1,0,0 12,12\n              L12,3A3,3 1,0,0 9,0\n              L7,0A2,2 1,0,0 5,2\n              L5,11A1,1 1,0,0 6,12\n              L9,12A1,1 1,0,0 10,11\n              L10,4A.25,.25 1,0,0 9,4 L9,11 6,11\n              L6,2A1,1 1,0,1 7,1\n              L9,1A2,2 1,0,1 11,3\n              L11,12A2,2 1,0,1 9,14\n              L6,14A2,2 1,0,1 4,12\n              L4,4A.25,.25 1,0,0 3,4Z" }));
            case "plus":
                return (h("path", { d: "M 6,0 9,0 9,6 15,6 15,9 9,9 9,15 6,15 6,9 0,9 0,6 6,6Z" }));
            case "search":
                return (h("path", { d: "M6,0 A1,1 0,0,0 6,12\n                  M6,12 A1,1 0,0,0 6,0\n                  M6,1.25 A1,1 0,0,1 6,10.75\n                  M6,10.75 A1,1 0,0,1 6,1.25\n                  M10,9 9,10 14,15 15,14Z" }));
            case "triangle":
                return h("path", { d: "M7.5,1 15,14 0,14 Z" });
        }
    }
    render() {
        const scaleX = this.scale * (this.flipHorizontal ? -1 : 1);
        const scaleY = this.scale * (this.flipVertical ? -1 : 1);
        return (h("svg", { xmlns: "http://www.w3.org/2000/svg", viewBox: "0 0 15 15" }, h("g", { transform: `
            translate(${7.5 - (15 / 2) * scaleX}, ${7.5 - (15 / 2) * scaleY})
            rotate(${this.rotate},${(15 / 2) * scaleX},${(15 / 2) * scaleY})
            scale(${scaleX},${scaleY})` }, this.get(this.type))));
    }
};
AppIconComponent.style = componentCss;

const componentCss$1 = ":root{--unit:12px;--padding:calc(var(--unit) / 2);--margin:1px;--border-width:var(--margin);--border:solid var(--border-width) var(--color);--border-radius:2px;--color:#444;--background-color:#ddd;--input-color:#333;--input-background-color:var(--highlight);--input-background-color-focus:var(--highlight-more);--action-color:#ccc;--action-color-alt:#222;--primary-color:#369;--primary-color-alt:#eee;--success-color:#494;--success-color-alt:#eee;--warning-color:#c82;--warning-color-alt:#eee;--error-color:#c22;--error-color-alt:#eee;--lowlight:rgba(0, 0, 0, .05);--lowlight-more:rgba(0, 0, 0, .25);--highlight:rgba(255, 255, 255, .25);--highlight-more:rgba(255, 255, 255, .5);--icon-size:.7em;--icon-color:currentColor;--icon-outline-size:.0075em;--icon-outline-color:var(--icon-color, currentColor)}:disabled{-webkit-filter:brightness(1.2) contrast(.5);filter:brightness(1.2) contrast(.5)}@media (prefers-color-scheme: dark){:root{--color:#ddd;--background-color:#333;--input-color:#eee;--highlight:rgba(0, 0, 0, .2);--highlight-more:rgba(0, 0, 0, .4);--lowlight:rgba(255, 255, 255, .05);--lowlight-more:rgba(255, 255, 255, .1);--action-color:#222;--action-color-alt:#eee}}:host{--host-unit:var(--unit, 12px);--host-padding:var(--padding, calc(var(--host-unit) / 2));--host-margin:var(--margin);--host-border-width:var(--border-width);--host-border:var(--border, solid var(--host-border-width) var(--host-color));--host-border-radius:var(--border-radius, 2px);--host-color:var(--color, #444);--host-background-color:var(--background-color, #f6f6f6);--host-action-color:var(--action-color, #ccc);--host-action-color-alt:var(--action-color-alt, #222);--host-primary-color:var(--primary-color, #369);--host-primary-color-alt:var(--primary-color-alt, #eee);--host-success-color:var(--success-color, #494);--host-success-color-alt:var(--success-color-alt, #eee);--host-warning-color:var(--warning-color, #c82);--host-warning-color-alt:var(--warning-color-alt, #eee);--host-error-color:var(--error-color, #c22);--host-error-color-alt:var(--error-color-alt, #eee);--host-lowlight:var(--lowlight, rgba(0, 0, 0, .05));--host-lowlight-more:var(--lowlight-more, rgba(0, 0, 0, .15));--host-highlight:var(--highlight, rgba(255, 255, 255, .25));--host-highlight-more:var(--highlight-more, rgba(255, 255, 255, .6));--host-button-padding:var(--host-padding) calc(var(--host-padding) * 2);--host-button-color:var(--host-action-color-alt);--host-button-background-color:var(--host-action-color);--host-button-border:var(--host-border);--host-button-border-radius:var(--host-border-radius);--host-button-primary-color:var(--host-primary-color-alt);--host-button-primary-background-color:var(--host-primary-color);--host-button-warning-color:var(--host-warning-color-alt);--host-button-warning-background-color:var(--host-warning-color);--host-input-padding:var(--host-padding);--host-input-color:var(--input-color);--host-input-background-color:var(--input-background-color);--host-input-background-color-focus:var(--input-background-color-focus);--host-input-border:var(--host-border);--host-input-border-radius:var(--host-border-radius);--host-label-padding:calc(var(--host-unit) / 2) calc(var(--host-unit) / 2) calc(var(--host-unit) / 4) calc(var(--host-unit) / 2)}button{padding:var(--host-button-padding);color:var(--host-button-color, var(--host-action-color-alt));background-color:var(--host-button-background-color, var(--host-action-color));border:var(--host-button-border, none);border-radius:var(--host-button-border-radius, 2px);margin:var(--host-margin);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}button app-icon{margin:-.25em}button.primary{color:var(--host-button-primary-color, var(--host-primary-color-alt));background-color:var(--host-button-primary-background-color, var(--host-primary-color))}button.warning{color:var(--host-button-warning-color, var(--host-warning-color-alt));background-color:var(--host-button-warning-background-color, var(--host-warning-color))}button.danger{color:var(--host-button-error-color, var(--host-error-color-alt));background-color:var(--host-button-error-background-color, var(--host-error-color))}input,textarea,select{padding:var(--host-input-padding);color:var(--host-input-color, var(--host-action-color-alt));background-color:var(--host-input-background-color, var(--host-action-color));border:var(--host-input-border, none);border-radius:var(--host-input-border-radius, 2px);margin:var(--host-margin)}input:focus,textarea:focus,select:focus{background-color:var(--host-input-background-color-focus, var(--host-action-color-focus))}label{padding:var(--host-label-padding);margin:var(--host-margin)}[slot=\"popup-header\"]{font-weight:700}.buttons{display:-ms-flexbox;display:flex;-ms-flex-pack:end;justify-content:flex-end}:host{display:-ms-flexbox;display:flex}input{-ms-flex:1 1 auto;flex:1 1 auto}:host([clear-button]) input{padding-right:1.5em}:host([icon-type]) input{padding-right:2em}:host([clear-button][icon-type]) input{padding-right:3.5em}.clear{background-color:transparent;position:absolute;right:1.75em;padding:var(--host-padding)}.icon{position:absolute;right:var(--host-padding);top:var(--host-padding);bottom:var(--host-padding);opacity:.8}";

const AppInputComponent = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.inputType = createEvent(this, "inputType", 7);
        this.inputChange = createEvent(this, "inputChange", 7);
        this.inputClear = createEvent(this, "inputClear", 7);
        this.clearButton = false;
        this.iconType = null;
        this.value = '';
    }
    render() {
        return h(Host, null, h("input", { value: this.value, placeholder: this.placeholder, onInput: (e) => this.inputType.emit(e.target.value), onChange: (e) => this.inputChange.emit(e.target.value), onKeyDown: e => {
                e.key === 'Escape' && this.inputClear.emit();
            } }), (this.clearButton && !!this.value) &&
            h("button", { class: "clear", onClick: () => this.inputClear.emit() }, h("app-icon", { type: "close" })), this.iconType &&
            h("app-icon", { class: "icon", type: this.iconType }));
    }
};
AppInputComponent.style = componentCss$1;

const componentCss$2 = ":root{--unit:12px;--padding:calc(var(--unit) / 2);--margin:1px;--border-width:var(--margin);--border:solid var(--border-width) var(--color);--border-radius:2px;--color:#444;--background-color:#ddd;--input-color:#333;--input-background-color:var(--highlight);--input-background-color-focus:var(--highlight-more);--action-color:#ccc;--action-color-alt:#222;--primary-color:#369;--primary-color-alt:#eee;--success-color:#494;--success-color-alt:#eee;--warning-color:#c82;--warning-color-alt:#eee;--error-color:#c22;--error-color-alt:#eee;--lowlight:rgba(0, 0, 0, .05);--lowlight-more:rgba(0, 0, 0, .25);--highlight:rgba(255, 255, 255, .25);--highlight-more:rgba(255, 255, 255, .5);--icon-size:.7em;--icon-color:currentColor;--icon-outline-size:.0075em;--icon-outline-color:var(--icon-color, currentColor)}:disabled{-webkit-filter:brightness(1.2) contrast(.5);filter:brightness(1.2) contrast(.5)}@media (prefers-color-scheme: dark){:root{--color:#ddd;--background-color:#333;--input-color:#eee;--highlight:rgba(0, 0, 0, .2);--highlight-more:rgba(0, 0, 0, .4);--lowlight:rgba(255, 255, 255, .05);--lowlight-more:rgba(255, 255, 255, .1);--action-color:#222;--action-color-alt:#eee}}:host{--host-unit:var(--unit, 12px);--host-padding:var(--padding, calc(var(--host-unit) / 2));--host-margin:var(--margin);--host-border-width:var(--border-width);--host-border:var(--border, solid var(--host-border-width) var(--host-color));--host-border-radius:var(--border-radius, 2px);--host-color:var(--color, #444);--host-background-color:var(--background-color, #f6f6f6);--host-action-color:var(--action-color, #ccc);--host-action-color-alt:var(--action-color-alt, #222);--host-primary-color:var(--primary-color, #369);--host-primary-color-alt:var(--primary-color-alt, #eee);--host-success-color:var(--success-color, #494);--host-success-color-alt:var(--success-color-alt, #eee);--host-warning-color:var(--warning-color, #c82);--host-warning-color-alt:var(--warning-color-alt, #eee);--host-error-color:var(--error-color, #c22);--host-error-color-alt:var(--error-color-alt, #eee);--host-lowlight:var(--lowlight, rgba(0, 0, 0, .05));--host-lowlight-more:var(--lowlight-more, rgba(0, 0, 0, .15));--host-highlight:var(--highlight, rgba(255, 255, 255, .25));--host-highlight-more:var(--highlight-more, rgba(255, 255, 255, .6));--host-button-padding:var(--host-padding) calc(var(--host-padding) * 2);--host-button-color:var(--host-action-color-alt);--host-button-background-color:var(--host-action-color);--host-button-border:var(--host-border);--host-button-border-radius:var(--host-border-radius);--host-button-primary-color:var(--host-primary-color-alt);--host-button-primary-background-color:var(--host-primary-color);--host-button-warning-color:var(--host-warning-color-alt);--host-button-warning-background-color:var(--host-warning-color);--host-input-padding:var(--host-padding);--host-input-color:var(--input-color);--host-input-background-color:var(--input-background-color);--host-input-background-color-focus:var(--input-background-color-focus);--host-input-border:var(--host-border);--host-input-border-radius:var(--host-border-radius);--host-label-padding:calc(var(--host-unit) / 2) calc(var(--host-unit) / 2) calc(var(--host-unit) / 4) calc(var(--host-unit) / 2)}button{padding:var(--host-button-padding);color:var(--host-button-color, var(--host-action-color-alt));background-color:var(--host-button-background-color, var(--host-action-color));border:var(--host-button-border, none);border-radius:var(--host-button-border-radius, 2px);margin:var(--host-margin);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}button app-icon{margin:-.25em}button.primary{color:var(--host-button-primary-color, var(--host-primary-color-alt));background-color:var(--host-button-primary-background-color, var(--host-primary-color))}button.warning{color:var(--host-button-warning-color, var(--host-warning-color-alt));background-color:var(--host-button-warning-background-color, var(--host-warning-color))}button.danger{color:var(--host-button-error-color, var(--host-error-color-alt));background-color:var(--host-button-error-background-color, var(--host-error-color))}input,textarea,select{padding:var(--host-input-padding);color:var(--host-input-color, var(--host-action-color-alt));background-color:var(--host-input-background-color, var(--host-action-color));border:var(--host-input-border, none);border-radius:var(--host-input-border-radius, 2px);margin:var(--host-margin)}input:focus,textarea:focus,select:focus{background-color:var(--host-input-background-color-focus, var(--host-action-color-focus))}label{padding:var(--host-label-padding);margin:var(--host-margin)}[slot=\"popup-header\"]{font-weight:700}.buttons{display:-ms-flexbox;display:flex;-ms-flex-pack:end;justify-content:flex-end}:host{display:inline-block;position:relative;--host-call-size:calc(.75 * var(--host-padding));--host-call-offset:calc(var(--host-padding) + var(--host-border-radius));--host-content-offset:calc(var(--host-call-offset) + var(--host-call-size))}:host([position=\"top\"]) .content{bottom:calc(100% + var(--host-call-size));left:50%;-webkit-transform:translateX(-50%);transform:translateX(-50%)}:host([position=\"top\"][shift=\"left\"]) .content{left:auto;right:50%;-webkit-transform:translateX(var(--host-content-offset));transform:translateX(var(--host-content-offset))}:host([position=\"top\"][shift=\"right\"]) .content{left:50%;-webkit-transform:translateX(calc(-1 * var(--host-content-offset)));transform:translateX(calc(-1 * var(--host-content-offset)))}:host([position=\"top\"]) .content::after{bottom:calc(-1 * var(--host-call-offset));border-top-color:var(--host-background-color);left:50%;-webkit-transform:translateX(-50%);transform:translateX(-50%)}:host([position=\"top\"][shift=\"left\"]) .content::after{left:auto;-webkit-transform:none;transform:none;right:var(--host-call-offset)}:host([position=\"top\"][shift=\"right\"]) .content::after{-webkit-transform:none;transform:none;left:var(--host-call-offset)}:host([position=\"right\"]) .content{left:calc(100% + var(--host-call-size));top:50%;-webkit-transform:translateY(-50%);transform:translateY(-50%)}:host([position=\"right\"][shift=\"left\"]) .content{top:auto;bottom:50%;-webkit-transform:translateY(var(--host-content-offset));transform:translateY(var(--host-content-offset))}:host([position=\"right\"][shift=\"right\"]) .content{top:50%;-webkit-transform:translateY(calc(-1 * var(--host-content-offset)));transform:translateY(calc(-1 * var(--host-content-offset)))}:host([position=\"right\"]) .content::after{left:calc(-1 * var(--host-call-offset));border-right-color:var(--host-background-color);top:50%;-webkit-transform:translateY(-50%);transform:translateY(-50%)}:host([position=\"right\"][shift=\"left\"]) .content::after{top:auto;-webkit-transform:none;transform:none;bottom:var(--host-call-offset)}:host([position=\"right\"][shift=\"right\"]) .content::after{-webkit-transform:none;transform:none;top:var(--host-call-offset)}:host([position=\"bottom\"]) .content{top:calc(100% + var(--host-call-size));left:50%;-webkit-transform:translateX(-50%);transform:translateX(-50%)}:host([position=\"bottom\"][shift=\"left\"]) .content{left:50%;-webkit-transform:translateX(calc(-1 * var(--host-content-offset)));transform:translateX(calc(-1 * var(--host-content-offset)))}:host([position=\"bottom\"][shift=\"right\"]) .content{left:auto;right:50%;-webkit-transform:translateX(var(--host-content-offset));transform:translateX(var(--host-content-offset))}:host([position=\"bottom\"]) .content::after{top:calc(-1 * var(--host-call-offset));border-bottom-color:var(--host-background-color);left:50%;-webkit-transform:translateX(-50%);transform:translateX(-50%)}:host([position=\"bottom\"][shift=\"left\"]) .content::after{-webkit-transform:none;transform:none;left:var(--host-call-offset)}:host([position=\"bottom\"][shift=\"right\"]) .content::after{-webkit-transform:none;transform:none;left:auto;right:var(--host-call-offset)}:host([position=\"left\"]) .content{right:calc(100% + var(--host-call-size));top:50%;-webkit-transform:translateY(-50%);transform:translateY(-50%)}:host([position=\"left\"][shift=\"left\"]) .content{top:50%;-webkit-transform:translateY(calc(-1 * var(--host-content-offset)));transform:translateY(calc(-1 * var(--host-content-offset)))}:host([position=\"left\"][shift=\"right\"]) .content{top:auto;bottom:50%;-webkit-transform:translateY(var(--host-content-offset));transform:translateY(var(--host-content-offset))}:host([position=\"left\"]) .content::after{right:calc(-1 * var(--host-call-offset));border-left-color:var(--host-background-color);top:50%;-webkit-transform:translateY(-50%);transform:translateY(-50%)}:host([position=\"left\"][shift=\"left\"]) .content::after{-webkit-transform:none;transform:none;top:var(--host-call-offset)}:host([position=\"left\"][shift=\"right\"]) .content::after{top:auto;-webkit-transform:none;transform:none;bottom:var(--host-call-offset)}.overlay{position:fixed;z-index:-1;top:0;right:0;bottom:0;left:0;background-color:var(--host-lowlight-more);opacity:0;-webkit-transition:opacity ease .2s;transition:opacity ease .2s;pointer-events:none}:host([is-open]) .overlay{opacity:1;z-index:1;pointer-events:all}.content{position:absolute;z-index:-1;-webkit-box-shadow:0 0 30px var(--host-lowlight-more);box-shadow:0 0 30px var(--host-lowlight-more);background-color:var(--host-background-color);border-radius:var(--host-border-radius);-webkit-transition:ease .4s;transition:ease .4s;-webkit-transition-property:margin-top, opacity;transition-property:margin-top, opacity;margin-top:var(--host-padding);opacity:0}:host([is-open]) .content{z-index:1;margin-top:0;opacity:1}.content::after{content:\" \";position:absolute;height:0;width:0;pointer-events:none;border:solid transparent;border-width:calc(.75 * var(--host-padding))}.header{background-color:var(--host-highlight);color:var(--host-primary-color);padding:var(--host-padding);border-bottom:solid 1px var(--host-lowlight)}.body{padding:var(--host-padding)}.footer{background-color:var(--host-lowlight);padding:var(--host-padding);border-top:solid 1px var(--host-highlight)}";

const AppPopupComponent = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.toggled = createEvent(this, "toggled", 7);
        this.isOpen = false;
        this.modal = false;
    }
    async toggle(open) {
        if (open == null)
            open = !this.isOpen;
        const e = this.toggled.emit(open);
        if (!e.defaultPrevented)
            this.isOpen = open;
    }
    render() {
        const hasHeader = this.element.querySelectorAll('[slot="popup-header"]').length > 0;
        const hasFooter = this.element.querySelectorAll('[slot="popup-footer"]').length > 0;
        return h(Host, { onClick: e => this.handleClicked(e) }, h("slot", null), h("div", { class: "overlay" }), h("div", { class: "content" }, hasHeader && h("header", { class: "header" }, h("slot", { name: "popup-header" })), h("div", { class: "body" }, h("slot", { name: "popup-body" })), hasFooter && h("footer", { class: "footer" }, h("slot", { name: "popup-footer" }))));
    }
    handleClicked(e) {
        e.stopPropagation();
        if (this.isOpen) {
            if (this.modal)
                return;
            const overlayElement = this.element.shadowRoot.querySelector('.overlay');
            if (!e.composedPath().some(et => et == overlayElement))
                return;
            this.toggle(false);
            return;
        }
        this.toggle(true);
    }
    get element() { return getElement(this); }
};
AppPopupComponent.style = componentCss$2;

const componentCss$3 = ":root{--unit:12px;--padding:calc(var(--unit) / 2);--margin:1px;--border-width:var(--margin);--border:solid var(--border-width) var(--color);--border-radius:2px;--color:#444;--background-color:#ddd;--input-color:#333;--input-background-color:var(--highlight);--input-background-color-focus:var(--highlight-more);--action-color:#ccc;--action-color-alt:#222;--primary-color:#369;--primary-color-alt:#eee;--success-color:#494;--success-color-alt:#eee;--warning-color:#c82;--warning-color-alt:#eee;--error-color:#c22;--error-color-alt:#eee;--lowlight:rgba(0, 0, 0, .05);--lowlight-more:rgba(0, 0, 0, .25);--highlight:rgba(255, 255, 255, .25);--highlight-more:rgba(255, 255, 255, .5);--icon-size:.7em;--icon-color:currentColor;--icon-outline-size:.0075em;--icon-outline-color:var(--icon-color, currentColor)}:disabled{-webkit-filter:brightness(1.2) contrast(.5);filter:brightness(1.2) contrast(.5)}@media (prefers-color-scheme: dark){:root{--color:#ddd;--background-color:#333;--input-color:#eee;--highlight:rgba(0, 0, 0, .2);--highlight-more:rgba(0, 0, 0, .4);--lowlight:rgba(255, 255, 255, .05);--lowlight-more:rgba(255, 255, 255, .1);--action-color:#222;--action-color-alt:#eee}}:host{--host-unit:var(--unit, 12px);--host-padding:var(--padding, calc(var(--host-unit) / 2));--host-margin:var(--margin);--host-border-width:var(--border-width);--host-border:var(--border, solid var(--host-border-width) var(--host-color));--host-border-radius:var(--border-radius, 2px);--host-color:var(--color, #444);--host-background-color:var(--background-color, #f6f6f6);--host-action-color:var(--action-color, #ccc);--host-action-color-alt:var(--action-color-alt, #222);--host-primary-color:var(--primary-color, #369);--host-primary-color-alt:var(--primary-color-alt, #eee);--host-success-color:var(--success-color, #494);--host-success-color-alt:var(--success-color-alt, #eee);--host-warning-color:var(--warning-color, #c82);--host-warning-color-alt:var(--warning-color-alt, #eee);--host-error-color:var(--error-color, #c22);--host-error-color-alt:var(--error-color-alt, #eee);--host-lowlight:var(--lowlight, rgba(0, 0, 0, .05));--host-lowlight-more:var(--lowlight-more, rgba(0, 0, 0, .15));--host-highlight:var(--highlight, rgba(255, 255, 255, .25));--host-highlight-more:var(--highlight-more, rgba(255, 255, 255, .6));--host-button-padding:var(--host-padding) calc(var(--host-padding) * 2);--host-button-color:var(--host-action-color-alt);--host-button-background-color:var(--host-action-color);--host-button-border:var(--host-border);--host-button-border-radius:var(--host-border-radius);--host-button-primary-color:var(--host-primary-color-alt);--host-button-primary-background-color:var(--host-primary-color);--host-button-warning-color:var(--host-warning-color-alt);--host-button-warning-background-color:var(--host-warning-color);--host-input-padding:var(--host-padding);--host-input-color:var(--input-color);--host-input-background-color:var(--input-background-color);--host-input-background-color-focus:var(--input-background-color-focus);--host-input-border:var(--host-border);--host-input-border-radius:var(--host-border-radius);--host-label-padding:calc(var(--host-unit) / 2) calc(var(--host-unit) / 2) calc(var(--host-unit) / 4) calc(var(--host-unit) / 2)}button{padding:var(--host-button-padding);color:var(--host-button-color, var(--host-action-color-alt));background-color:var(--host-button-background-color, var(--host-action-color));border:var(--host-button-border, none);border-radius:var(--host-button-border-radius, 2px);margin:var(--host-margin);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}button app-icon{margin:-.25em}button.primary{color:var(--host-button-primary-color, var(--host-primary-color-alt));background-color:var(--host-button-primary-background-color, var(--host-primary-color))}button.warning{color:var(--host-button-warning-color, var(--host-warning-color-alt));background-color:var(--host-button-warning-background-color, var(--host-warning-color))}button.danger{color:var(--host-button-error-color, var(--host-error-color-alt));background-color:var(--host-button-error-background-color, var(--host-error-color))}input,textarea,select{padding:var(--host-input-padding);color:var(--host-input-color, var(--host-action-color-alt));background-color:var(--host-input-background-color, var(--host-action-color));border:var(--host-input-border, none);border-radius:var(--host-input-border-radius, 2px);margin:var(--host-margin)}input:focus,textarea:focus,select:focus{background-color:var(--host-input-background-color-focus, var(--host-action-color-focus))}label{padding:var(--host-label-padding);margin:var(--host-margin)}[slot=\"popup-header\"]{font-weight:700}.buttons{display:-ms-flexbox;display:flex;-ms-flex-pack:end;justify-content:flex-end}:host{position:fixed;top:0;right:0;bottom:0;left:0;display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column}:host::before{z-index:-1;content:'';position:fixed;bottom:var(--host-padding);left:var(--host-padding);width:3em;height:3em;background-image:url(/assets/icon.svg);background-position:left bottom;background-repeat:no-repeat}.intro{padding:var(--host-padding);-ms-flex-align:center;align-items:center;margin:auto;max-width:15em;color:var(--primary-color)}.hosts{-ms-flex:1 1 auto;flex:1 1 auto;display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column;overflow:hidden}.hosts ul{-ms-flex:1 1 auto;flex:1 1 auto;display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column;-ms-flex-pack:start;justify-content:flex-start;overflow:hidden;margin:0;padding:0}.host{-ms-flex:0 0 auto;flex:0 0 auto;display:-ms-flexbox;display:flex;-ms-flex-pack:start;justify-content:flex-start;overflow:hidden;padding:var(--host-padding);border-bottom:solid 1px var(--host-lowlight)}.host-open{-ms-flex:1 1 auto;flex:1 1 auto;background:-webkit-gradient(linear, left top, left bottom, from(var(--host-highlight)), to(var(--host-highlight-more)));background:linear-gradient(to bottom, var(--host-highlight), var(--host-highlight-more));border-bottom:solid 1px var(--host-lowlight-more)}.host-actions{-ms-flex:0 0 auto;flex:0 0 auto;display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column}smtp-host{-ms-flex:1 1 auto;flex:1 1 auto}main{-ms-flex:1 1 auto;flex:1 1 auto;max-height:100%;display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column;overflow-y:auto}.hosts-actions{-ms-flex-item-align:end;align-self:flex-end;padding:var(--host-padding)}pre{font-size:.75em;color:rgba(0, 0, 0, .75);white-space:pre-wrap;word-break:break-all}impostor-hub{height:1.25em}.buttons app-popup{margin-right:auto}";

const AppRoot = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.logger = globalThis.getLogger('AppRoot');
        this.store = getContext(this, "store");
    }
    async componentWillLoad() {
        this.store.setStore(configureStore({}, () => this.hubAction()));
        this.store.mapDispatchToProps(this, {
            dispatch,
            initWorkerState,
            loadWorkerState,
            addHost,
            removeHost,
            startHost,
            stopHost,
            openHost,
            updateHost,
            toggleHostConfiguration,
            searchHostMessages,
            deleteHostMessage,
            loadHostMessage,
            startupWorker,
            shutdownWorker
        });
        this.store.mapStateToProps(this, (state) => {
            return { state };
        });
    }
    hubAction() {
        return (next) => async (action) => {
            this.logger.debug('hubAction', action.type, { action });
            if (action.sendToHub)
                await this.hub.sendAsync(action);
            else {
                switch (action.type) {
                    case Types.STARTUP_WORKER:
                        location.href = 'smtp-impostor:://start';
                        break;
                    case Types.WORKER_STATE:
                        if (this.newHostId) {
                            const hostId = this.newHostId;
                            requestAnimationFrame(() => {
                                this.openHost(hostId);
                                this.toggleHostConfiguration(hostId, true);
                            });
                            this.newHostId = null;
                        }
                        break;
                    case Types.HOST_MESSAGE_RECEIVED:
                        if (this.state.worker.openHostId === action.model.hostId)
                            this.searchHostMessages(action.model.hostId, DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA);
                        break;
                    case Types.HOST_MESSAGE:
                        const file = new File([action.model.content], `${action.model.id}.eml`, { type: 'message/rfc822' });
                        const url = URL.createObjectURL(file);
                        location.assign(url);
                        URL.revokeObjectURL(url);
                        break;
                }
                await next(action);
            }
        };
    }
    render() {
        return h(Host, null, h("main", null, !this.state.worker.hosts
            ? h("div", { class: "intro" }, h("strong", null, "Welcome, start the worker service below to begin catching emails"))
            : h("div", { class: "hosts" }, h("ul", null, this.state.worker.hosts.map(host => (h("li", { key: host.id, class: { "host": true, "host-open": this.state.worker.openHostId === host.id } }, h("smtp-host", { value: host, showMessages: this.state.worker.openHostId === host.id, onStartHost: e => this.startHost(e.detail.id), onStopHost: e => this.stopHost(e.detail.id), onOpenHost: () => this.openHost(this.state.worker.openHostId === host.id ? null : host.id), onSearchHostMessages: e => this.searchHostMessages(e.detail.id, e.detail.criteria), onDeleteHostMessage: e => this.deleteHostMessage(e.detail.id, e.detail.messageId), onOpenHostMessage: e => this.loadHostMessage(e.detail.id, e.detail.messageId) }), h("div", { class: "host-actions" }, h("button", { class: "toggle-readonly", type: "button", onClick: () => this.openHost(this.state.worker.openHostId === host.id ? null : host.id) }, h("app-icon", { type: "triangle", scale: .65, rotate: this.state.worker.openHostId === host.id ? 0 : 180 })), host.showConfiguration, this.state.worker.openHostId === host.id &&
                h("app-popup", { position: "left", shift: "left", isOpen: host.showConfiguration }, h("button", { class: "remove-host", onClick: () => this.toggleHostConfiguration(host.id, true) }, h("app-icon", { type: "cog", scale: 1.667 })), h("div", { slot: "popup-header" }, "Host Configuration"), h("smtp-host-configuration", { slot: "popup-body", value: host, onUpdateHost: e => this.updateHost(e.detail) }), h("div", { class: "buttons", slot: "popup-footer" }, h("app-popup", { position: "right" }, h("button", { class: "delete warning" }, h("app-icon", { type: "delete" }), " Delete"), h("button", { slot: "popup-body", class: "confirm danger", onClick: () => this.removeHost(host.id) }, "Confirm"))))))))), h("div", { class: "hosts-actions" }, h("button", { class: "add-host primary", onClick: () => {
                    this.newHostId = newId();
                    this.addHost({ id: this.newHostId });
                } }, h("app-icon", { type: "plus" }))))), h("impostor-hub", { ref: el => (this.hub = el), onStatusChanged: e => this.handleHubStatusChangedAsync(e), onMessageReceived: e => this.dispatch(e.detail), onStartupWorker: () => this.startupWorker(), onShutdownWorker: () => this.shutdownWorker() }));
    }
    componentDidLoad() {
        this.hub.connectAsync();
    }
    async handleHubStatusChangedAsync(e) {
        this.logger.debug('handleHubStatusChangedAsync', { e });
        switch (e.detail) {
            default:
                this.initWorkerState();
                break;
            case HubStatus.connected:
                this.loadWorkerState();
                break;
        }
    }
};
AppRoot.style = componentCss$3;

const componentCss$4 = ":root{--unit:12px;--padding:calc(var(--unit) / 2);--margin:1px;--border-width:var(--margin);--border:solid var(--border-width) var(--color);--border-radius:2px;--color:#444;--background-color:#ddd;--input-color:#333;--input-background-color:var(--highlight);--input-background-color-focus:var(--highlight-more);--action-color:#ccc;--action-color-alt:#222;--primary-color:#369;--primary-color-alt:#eee;--success-color:#494;--success-color-alt:#eee;--warning-color:#c82;--warning-color-alt:#eee;--error-color:#c22;--error-color-alt:#eee;--lowlight:rgba(0, 0, 0, .05);--lowlight-more:rgba(0, 0, 0, .25);--highlight:rgba(255, 255, 255, .25);--highlight-more:rgba(255, 255, 255, .5);--icon-size:.7em;--icon-color:currentColor;--icon-outline-size:.0075em;--icon-outline-color:var(--icon-color, currentColor)}:disabled{-webkit-filter:brightness(1.2) contrast(.5);filter:brightness(1.2) contrast(.5)}@media (prefers-color-scheme: dark){:root{--color:#ddd;--background-color:#333;--input-color:#eee;--highlight:rgba(0, 0, 0, .2);--highlight-more:rgba(0, 0, 0, .4);--lowlight:rgba(255, 255, 255, .05);--lowlight-more:rgba(255, 255, 255, .1);--action-color:#222;--action-color-alt:#eee}}:host{--host-unit:var(--unit, 12px);--host-padding:var(--padding, calc(var(--host-unit) / 2));--host-margin:var(--margin);--host-border-width:var(--border-width);--host-border:var(--border, solid var(--host-border-width) var(--host-color));--host-border-radius:var(--border-radius, 2px);--host-color:var(--color, #444);--host-background-color:var(--background-color, #f6f6f6);--host-action-color:var(--action-color, #ccc);--host-action-color-alt:var(--action-color-alt, #222);--host-primary-color:var(--primary-color, #369);--host-primary-color-alt:var(--primary-color-alt, #eee);--host-success-color:var(--success-color, #494);--host-success-color-alt:var(--success-color-alt, #eee);--host-warning-color:var(--warning-color, #c82);--host-warning-color-alt:var(--warning-color-alt, #eee);--host-error-color:var(--error-color, #c22);--host-error-color-alt:var(--error-color-alt, #eee);--host-lowlight:var(--lowlight, rgba(0, 0, 0, .05));--host-lowlight-more:var(--lowlight-more, rgba(0, 0, 0, .15));--host-highlight:var(--highlight, rgba(255, 255, 255, .25));--host-highlight-more:var(--highlight-more, rgba(255, 255, 255, .6));--host-button-padding:var(--host-padding) calc(var(--host-padding) * 2);--host-button-color:var(--host-action-color-alt);--host-button-background-color:var(--host-action-color);--host-button-border:var(--host-border);--host-button-border-radius:var(--host-border-radius);--host-button-primary-color:var(--host-primary-color-alt);--host-button-primary-background-color:var(--host-primary-color);--host-button-warning-color:var(--host-warning-color-alt);--host-button-warning-background-color:var(--host-warning-color);--host-input-padding:var(--host-padding);--host-input-color:var(--input-color);--host-input-background-color:var(--input-background-color);--host-input-background-color-focus:var(--input-background-color-focus);--host-input-border:var(--host-border);--host-input-border-radius:var(--host-border-radius);--host-label-padding:calc(var(--host-unit) / 2) calc(var(--host-unit) / 2) calc(var(--host-unit) / 4) calc(var(--host-unit) / 2)}button{padding:var(--host-button-padding);color:var(--host-button-color, var(--host-action-color-alt));background-color:var(--host-button-background-color, var(--host-action-color));border:var(--host-button-border, none);border-radius:var(--host-button-border-radius, 2px);margin:var(--host-margin);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}button app-icon{margin:-.25em}button.primary{color:var(--host-button-primary-color, var(--host-primary-color-alt));background-color:var(--host-button-primary-background-color, var(--host-primary-color))}button.warning{color:var(--host-button-warning-color, var(--host-warning-color-alt));background-color:var(--host-button-warning-background-color, var(--host-warning-color))}button.danger{color:var(--host-button-error-color, var(--host-error-color-alt));background-color:var(--host-button-error-background-color, var(--host-error-color))}input,textarea,select{padding:var(--host-input-padding);color:var(--host-input-color, var(--host-action-color-alt));background-color:var(--host-input-background-color, var(--host-action-color));border:var(--host-input-border, none);border-radius:var(--host-input-border-radius, 2px);margin:var(--host-margin)}input:focus,textarea:focus,select:focus{background-color:var(--host-input-background-color-focus, var(--host-action-color-focus))}label{padding:var(--host-label-padding);margin:var(--host-margin)}[slot=\"popup-header\"]{font-weight:700}.buttons{display:-ms-flexbox;display:flex;-ms-flex-pack:end;justify-content:flex-end}:host{--host-worker-status-size:var(--worker-status-size, .75em);display:-ms-inline-flexbox;display:inline-flex}.status{position:relative;left:3em;bottom:.5em}.status .icon{position:relative;display:inline-block;width:var(--host-worker-status-size);height:var(--host-worker-status-size);overflow:hidden;text-indent:1000px;border-radius:50%;background-color:var(--host-lowlight-more)}.status.status-1 .icon{background-color:var(--host-warning-color)}.status.status-2 .icon{background-color:var(--host-success-color);cursor:pointer}.status.status-3 .icon{background-color:var(--host-error-color)}";

const RECONNECT_INIT = 200;
const RECONNECT_MAX = 15000;
const ImpostorHubComponent = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.statusChanged = createEvent(this, "statusChanged", 7);
        this.messageReceived = createEvent(this, "messageReceived", 7);
        this.startupWorker = createEvent(this, "startupWorker", 7);
        this.shutdownWorker = createEvent(this, "shutdownWorker", 7);
        this.logger = globalThis.getLogger('ImpostorHubComponent');
        this.reconnectIn = 200;
        this.socketProvider = hubSocketProvider;
        this.url = `ws://localhost:62525/hub`;
        this.status = HubStatus.disconnected;
    }
    async connectAsync(url) {
        if (url)
            this.url = url;
        this.logger.info('connecting', { url: this.url });
        try {
            if (this.socket)
                this.disconnectAsync();
            this.socket = this.socketProvider(this.url);
            this.socket.onopen = (e) => {
                this.logger.debug('socket.onopen', { e });
                this.reconnectIn = RECONNECT_INIT;
                this.setStatus(HubStatus.connected);
            };
            this.socket.onclose = (e) => {
                this.logger.info('socket.onclose', { e });
                this.disconnectAsync();
                this.retryConnect();
            };
            this.socket.onerror = (e) => {
                this.logger.warn('socket.onerror', { e });
                this.setStatus(HubStatus.error);
            };
            this.socket.onmessage = (e) => {
                this.logger.debug('socket.onmessage', { e });
                const message = JSON.parse(e.data);
                this.messageReceived.emit({
                    type: message.type,
                    model: message.data && JSON.parse(message.data)
                });
            };
        }
        catch (err) {
            this.logger.error('connect', err);
            this.setStatus(HubStatus.error);
            this.retryConnect();
        }
    }
    retryConnect() {
        this.logger.info('retryConnect', this.reconnectIn);
        setTimeout(() => this.connectAsync(), this.reconnectIn);
        this.reconnectIn *= 2;
        if (this.reconnectIn > RECONNECT_MAX)
            this.reconnectIn = RECONNECT_MAX;
    }
    async sendAsync(message) {
        if (!this.socket)
            throw new Error('cannot send, not connected');
        this.socket.send(JSON.stringify({
            type: message.type,
            data: message.model ? JSON.stringify(message.model) : undefined
        }));
    }
    async disconnectAsync() {
        this.logger.debug('disconnectAsync');
        if (this.socket.readyState < WebSocket.CLOSING) {
            this.logger.debug('disconnectAsync socket.close');
            this.socket.close();
        }
        this.socket = null;
        this.setStatus(HubStatus.disconnected);
    }
    render() {
        return h("div", { class: `status status-${this.status}` }, h("app-popup", { "is-open": true, position: "top", shift: "right" }, h("span", { class: "icon" }, HubStatus[this.status]), h("div", { slot: "popup-header" }, "Worker"), h("div", { slot: "popup-body" }, "Status:\u00A0", hubStatusDisplay(this.status)), h("div", { slot: "popup-footer", class: "buttons" }, this.status === HubStatus.disconnected
            ? h("button", { class: "primary", onClick: () => {
                    this.startupWorker.emit();
                    this.status = HubStatus.working;
                } }, "Start")
            : h("button", { class: "primary", disabled: this.status === HubStatus.working, onClick: () => {
                    this.shutdownWorker.emit();
                    this.status = HubStatus.working;
                } }, "Shutdown"))));
    }
    setStatus(value) {
        this.status = value;
        this.statusChanged.emit(value);
    }
};
ImpostorHubComponent.style = componentCss$4;

const componentCss$5 = ":root{--unit:12px;--padding:calc(var(--unit) / 2);--margin:1px;--border-width:var(--margin);--border:solid var(--border-width) var(--color);--border-radius:2px;--color:#444;--background-color:#ddd;--input-color:#333;--input-background-color:var(--highlight);--input-background-color-focus:var(--highlight-more);--action-color:#ccc;--action-color-alt:#222;--primary-color:#369;--primary-color-alt:#eee;--success-color:#494;--success-color-alt:#eee;--warning-color:#c82;--warning-color-alt:#eee;--error-color:#c22;--error-color-alt:#eee;--lowlight:rgba(0, 0, 0, .05);--lowlight-more:rgba(0, 0, 0, .25);--highlight:rgba(255, 255, 255, .25);--highlight-more:rgba(255, 255, 255, .5);--icon-size:.7em;--icon-color:currentColor;--icon-outline-size:.0075em;--icon-outline-color:var(--icon-color, currentColor)}:disabled{-webkit-filter:brightness(1.2) contrast(.5);filter:brightness(1.2) contrast(.5)}@media (prefers-color-scheme: dark){:root{--color:#ddd;--background-color:#333;--input-color:#eee;--highlight:rgba(0, 0, 0, .2);--highlight-more:rgba(0, 0, 0, .4);--lowlight:rgba(255, 255, 255, .05);--lowlight-more:rgba(255, 255, 255, .1);--action-color:#222;--action-color-alt:#eee}}:host{--host-unit:var(--unit, 12px);--host-padding:var(--padding, calc(var(--host-unit) / 2));--host-margin:var(--margin);--host-border-width:var(--border-width);--host-border:var(--border, solid var(--host-border-width) var(--host-color));--host-border-radius:var(--border-radius, 2px);--host-color:var(--color, #444);--host-background-color:var(--background-color, #f6f6f6);--host-action-color:var(--action-color, #ccc);--host-action-color-alt:var(--action-color-alt, #222);--host-primary-color:var(--primary-color, #369);--host-primary-color-alt:var(--primary-color-alt, #eee);--host-success-color:var(--success-color, #494);--host-success-color-alt:var(--success-color-alt, #eee);--host-warning-color:var(--warning-color, #c82);--host-warning-color-alt:var(--warning-color-alt, #eee);--host-error-color:var(--error-color, #c22);--host-error-color-alt:var(--error-color-alt, #eee);--host-lowlight:var(--lowlight, rgba(0, 0, 0, .05));--host-lowlight-more:var(--lowlight-more, rgba(0, 0, 0, .15));--host-highlight:var(--highlight, rgba(255, 255, 255, .25));--host-highlight-more:var(--highlight-more, rgba(255, 255, 255, .6));--host-button-padding:var(--host-padding) calc(var(--host-padding) * 2);--host-button-color:var(--host-action-color-alt);--host-button-background-color:var(--host-action-color);--host-button-border:var(--host-border);--host-button-border-radius:var(--host-border-radius);--host-button-primary-color:var(--host-primary-color-alt);--host-button-primary-background-color:var(--host-primary-color);--host-button-warning-color:var(--host-warning-color-alt);--host-button-warning-background-color:var(--host-warning-color);--host-input-padding:var(--host-padding);--host-input-color:var(--input-color);--host-input-background-color:var(--input-background-color);--host-input-background-color-focus:var(--input-background-color-focus);--host-input-border:var(--host-border);--host-input-border-radius:var(--host-border-radius);--host-label-padding:calc(var(--host-unit) / 2) calc(var(--host-unit) / 2) calc(var(--host-unit) / 4) calc(var(--host-unit) / 2)}button{padding:var(--host-button-padding);color:var(--host-button-color, var(--host-action-color-alt));background-color:var(--host-button-background-color, var(--host-action-color));border:var(--host-button-border, none);border-radius:var(--host-button-border-radius, 2px);margin:var(--host-margin);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}button app-icon{margin:-.25em}button.primary{color:var(--host-button-primary-color, var(--host-primary-color-alt));background-color:var(--host-button-primary-background-color, var(--host-primary-color))}button.warning{color:var(--host-button-warning-color, var(--host-warning-color-alt));background-color:var(--host-button-warning-background-color, var(--host-warning-color))}button.danger{color:var(--host-button-error-color, var(--host-error-color-alt));background-color:var(--host-button-error-background-color, var(--host-error-color))}input,textarea,select{padding:var(--host-input-padding);color:var(--host-input-color, var(--host-action-color-alt));background-color:var(--host-input-background-color, var(--host-action-color));border:var(--host-input-border, none);border-radius:var(--host-input-border-radius, 2px);margin:var(--host-margin)}input:focus,textarea:focus,select:focus{background-color:var(--host-input-background-color-focus, var(--host-action-color-focus))}label{padding:var(--host-label-padding);margin:var(--host-margin)}[slot=\"popup-header\"]{font-weight:700}.buttons{display:-ms-flexbox;display:flex;-ms-flex-pack:end;justify-content:flex-end}:host{display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column;-ms-flex-pack:start;justify-content:flex-start;overflow:visible}.actions{-ms-flex:1 0 auto;flex:1 0 auto;display:-ms-flexbox;display:flex}input{width:auto;-ms-flex:1 1 auto;flex:1 1 auto}header{display:-ms-flexbox;display:flex;-ms-flex:0 0 auto;flex:0 0 auto}header .name{-ms-flex:1 1 auto;flex:1 1 auto;position:relative;padding:var(--host-padding) 3em 0 var(--host-padding)}header .message-count{position:absolute;top:var(--host-padding);right:var(--host-padding)}header .message-count::before{content:'('}header .message-count::after{content:')'}header .actions{-ms-flex:0 0 auto;flex:0 0 auto}.toggle-state{position:relative;-ms-flex-item-align:start;align-self:flex-start;width:4.5em;padding-left:calc(var(--host-padding) + var(--host-unit))}.toggle-state::before{content:'';position:absolute;top:0;left:0;width:var(--host-unit);bottom:0;background-color:var(--host-lowlight);border-radius:var(--host-border-radius)}:host(.started) .toggle-state::before{background-color:var(--host-success-color)}:host(.receiving) .toggle-state::before{background-color:var(--host-success-color);-webkit-filter:brightness(1.25);filter:brightness(1.25)}:host(.errored) .toggle-state::before{background-color:var(--host-warning-color)}:host(.stopped) .toggle-state::before{background-color:var(--host-error-color)}.messages{-ms-flex:0 1 auto;flex:0 1 auto;display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column;overflow:hidden}.messages-toolbar{position:relative;-ms-flex:0 0 auto;flex:0 0 auto;display:-ms-flexbox;display:flex}.messages-toolbar app-input{-ms-flex:1 1 auto;flex:1 1 auto}.messages-list{-ms-flex:1 1 auto;flex:1 1 auto;display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column;overflow-y:auto;margin:var(--host-padding) 0;padding:0}.message{position:relative;-ms-flex:1 0 auto;flex:1 0 auto;display:-ms-flexbox;display:flex;-ms-flex-wrap:wrap;flex-wrap:wrap;padding:var(--host-padding);border-top:solid 1px var(--host-lowlight);border-radius:var(--host-border-radius)}.message:hover{background-color:var(--host-highlight-more)}.message-from{-ms-flex:1 1 auto;flex:1 1 auto;font-size:.9em;margin-right:auto}.message-date{-ms-flex:0 0 auto;flex:0 0 auto;font-size:.8em}.message-subject{width:100%;font-size:1.1em;margin:var(--host-padding) 2em var(--host-padding) 0}.delete-message{position:absolute;right:var(--host-padding);bottom:var(--host-padding)}";

const SMTPHostComponent = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.startHost = createEvent(this, "startHost", 7);
        this.stopHost = createEvent(this, "stopHost", 7);
        this.openHost = createEvent(this, "openHost", 7);
        this.searchHostMessages = createEvent(this, "searchHostMessages", 7);
        this.deleteHostMessage = createEvent(this, "deleteHostMessage", 7);
        this.openHostMessage = createEvent(this, "openHostMessage", 7);
        this.logger = globalThis.getLogger('SMTPHostComponent');
        this.value = null;
        this.showMessages = false;
        this.showConfiguration = false;
        this.messagesSearchCriteria = DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA;
    }
    async toggleState(start) {
        start = start == null
            ? !hostIsRunning(this.value)
            : !!start;
        if (start)
            this.startHost.emit(this.value);
        else
            this.stopHost.emit(this.value);
    }
    async searchMessages(criteria = {}, debounce = 0) {
        this.messagesSearchCriteria = Object.assign(Object.assign({}, this.messagesSearchCriteria), criteria);
        if (this.messagesSearchTimer)
            clearTimeout(this.messagesSearchTimer);
        this.messagesSearchTimer = setTimeout(() => {
            this.searchHostMessages.emit({
                id: this.value.id,
                criteria: this.messagesSearchCriteria
            });
        }, debounce);
    }
    render() {
        if (this.value == null)
            return '(host not set)';
        return (h(Host, { class: HostStatus[this.value.state].toLowerCase() }, h("header", null, h("div", { class: "name", onDblClick: () => this.openHost.emit({ id: this.value.id }) }, this.value.name, h("small", { class: "message-count" }, this.value.messagesCount)), h("div", { class: "actions" }, h("button", { class: "toggle-state", type: "button", onClick: () => this.toggleState() }, h("span", null, hostIsRunning(this.value) ? "Stop" : "Start")))), h("div", { class: "messages" }, this.showMessages && this.renderMessages())));
    }
    renderMessages() {
        if (this.value.messages == null) {
            this.value.messages = [];
            this.searchMessages();
        }
        return h(Frag, null, h("div", { class: "messages-toolbar" }, h("app-input", { "clear-button": true, "icon-type": "search", value: this.messagesSearchCriteria.text, placeholder: "search messages", onInputType: (e) => this.searchMessages({ text: e.detail }, 500), onInputClear: () => this.searchMessages({ text: '' }, 0) })), h("ul", { class: "messages-list" }, this.value.messages && this.value.messages
            .map(message => h("li", { class: "message", "data-id": message.id, onDblClick: () => this.openHostMessage.emit({ id: this.value.id, messageId: message.id }) }, h("div", { class: "message-from" }, message.from), h("div", { class: "message-date" }, new Date(message.date).toLocaleString(), h("button", { class: "delete-message danger", type: "button", onClick: () => this.deleteHostMessage.emit({
                id: this.value.id,
                messageId: message.id
            }) }, h("app-icon", { type: "close" }))), h("div", { class: "message-subject" }, message.subject)))));
    }
};
SMTPHostComponent.style = componentCss$5;

const componentCss$6 = ":root{--unit:12px;--padding:calc(var(--unit) / 2);--margin:1px;--border-width:var(--margin);--border:solid var(--border-width) var(--color);--border-radius:2px;--color:#444;--background-color:#ddd;--input-color:#333;--input-background-color:var(--highlight);--input-background-color-focus:var(--highlight-more);--action-color:#ccc;--action-color-alt:#222;--primary-color:#369;--primary-color-alt:#eee;--success-color:#494;--success-color-alt:#eee;--warning-color:#c82;--warning-color-alt:#eee;--error-color:#c22;--error-color-alt:#eee;--lowlight:rgba(0, 0, 0, .05);--lowlight-more:rgba(0, 0, 0, .25);--highlight:rgba(255, 255, 255, .25);--highlight-more:rgba(255, 255, 255, .5);--icon-size:.7em;--icon-color:currentColor;--icon-outline-size:.0075em;--icon-outline-color:var(--icon-color, currentColor)}:disabled{-webkit-filter:brightness(1.2) contrast(.5);filter:brightness(1.2) contrast(.5)}@media (prefers-color-scheme: dark){:root{--color:#ddd;--background-color:#333;--input-color:#eee;--highlight:rgba(0, 0, 0, .2);--highlight-more:rgba(0, 0, 0, .4);--lowlight:rgba(255, 255, 255, .05);--lowlight-more:rgba(255, 255, 255, .1);--action-color:#222;--action-color-alt:#eee}}:host{--host-unit:var(--unit, 12px);--host-padding:var(--padding, calc(var(--host-unit) / 2));--host-margin:var(--margin);--host-border-width:var(--border-width);--host-border:var(--border, solid var(--host-border-width) var(--host-color));--host-border-radius:var(--border-radius, 2px);--host-color:var(--color, #444);--host-background-color:var(--background-color, #f6f6f6);--host-action-color:var(--action-color, #ccc);--host-action-color-alt:var(--action-color-alt, #222);--host-primary-color:var(--primary-color, #369);--host-primary-color-alt:var(--primary-color-alt, #eee);--host-success-color:var(--success-color, #494);--host-success-color-alt:var(--success-color-alt, #eee);--host-warning-color:var(--warning-color, #c82);--host-warning-color-alt:var(--warning-color-alt, #eee);--host-error-color:var(--error-color, #c22);--host-error-color-alt:var(--error-color-alt, #eee);--host-lowlight:var(--lowlight, rgba(0, 0, 0, .05));--host-lowlight-more:var(--lowlight-more, rgba(0, 0, 0, .15));--host-highlight:var(--highlight, rgba(255, 255, 255, .25));--host-highlight-more:var(--highlight-more, rgba(255, 255, 255, .6));--host-button-padding:var(--host-padding) calc(var(--host-padding) * 2);--host-button-color:var(--host-action-color-alt);--host-button-background-color:var(--host-action-color);--host-button-border:var(--host-border);--host-button-border-radius:var(--host-border-radius);--host-button-primary-color:var(--host-primary-color-alt);--host-button-primary-background-color:var(--host-primary-color);--host-button-warning-color:var(--host-warning-color-alt);--host-button-warning-background-color:var(--host-warning-color);--host-input-padding:var(--host-padding);--host-input-color:var(--input-color);--host-input-background-color:var(--input-background-color);--host-input-background-color-focus:var(--input-background-color-focus);--host-input-border:var(--host-border);--host-input-border-radius:var(--host-border-radius);--host-label-padding:calc(var(--host-unit) / 2) calc(var(--host-unit) / 2) calc(var(--host-unit) / 4) calc(var(--host-unit) / 2)}button{padding:var(--host-button-padding);color:var(--host-button-color, var(--host-action-color-alt));background-color:var(--host-button-background-color, var(--host-action-color));border:var(--host-button-border, none);border-radius:var(--host-button-border-radius, 2px);margin:var(--host-margin);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}button app-icon{margin:-.25em}button.primary{color:var(--host-button-primary-color, var(--host-primary-color-alt));background-color:var(--host-button-primary-background-color, var(--host-primary-color))}button.warning{color:var(--host-button-warning-color, var(--host-warning-color-alt));background-color:var(--host-button-warning-background-color, var(--host-warning-color))}button.danger{color:var(--host-button-error-color, var(--host-error-color-alt));background-color:var(--host-button-error-background-color, var(--host-error-color))}input,textarea,select{padding:var(--host-input-padding);color:var(--host-input-color, var(--host-action-color-alt));background-color:var(--host-input-background-color, var(--host-action-color));border:var(--host-input-border, none);border-radius:var(--host-input-border-radius, 2px);margin:var(--host-margin)}input:focus,textarea:focus,select:focus{background-color:var(--host-input-background-color-focus, var(--host-action-color-focus))}label{padding:var(--host-label-padding);margin:var(--host-margin)}[slot=\"popup-header\"]{font-weight:700}.buttons{display:-ms-flexbox;display:flex;-ms-flex-pack:end;justify-content:flex-end}:host{display:-ms-flexbox;display:flex;-ms-flex-wrap:wrap;flex-wrap:wrap}.control{display:-ms-flexbox;display:flex;-ms-flex-direction:column;flex-direction:column;justify-items:stretch;position:relative}.control.ip{-ms-flex:1 0 auto;flex:1 0 auto;width:4em}.control.port{-ms-flex:0 0 auto;flex:0 0 auto;width:3em}.control.port input{text-align:center}.control.name{-ms-flex:1 1 auto;flex:1 1 auto;width:100%}";

const SMTPHostConfigurationComponent = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.updateHost = createEvent(this, "updateHost", 7);
        this.logger = globalThis.getLogger('SMTPHostConfigurationComponent');
        this.value = null;
    }
    render() {
        return h(Frag, null, h("div", { class: "control ip" }, h("label", null, "IP Address"), h("input", { name: "ip", value: this.value.ip, onChange: (e) => this.updateHost.emit({ id: this.value.id, ip: e.target.value }) })), h("div", { class: "control port" }, h("label", null, "Port"), h("input", { name: "port", value: this.value.port, onChange: (e) => this.updateHost.emit({ id: this.value.id, port: e.target.value }) })), h("div", { class: "control name" }, h("label", null, "Friendly Name"), h("input", { name: "name", value: this.value.name, onChange: (e) => this.updateHost.emit({ id: this.value.id, name: e.target.value }) })), h("div", { class: "control name" }, h("label", null, "Max Messages"), h("input", { name: "maxMessages", value: this.value.maxMessages || '', onChange: (e) => this.updateHost.emit({ id: this.value.id, maxMessages: e.target.value || 0 }) })));
    }
};
SMTPHostConfigurationComponent.style = componentCss$6;

export { AppIconComponent as app_icon, AppInputComponent as app_input, AppPopupComponent as app_popup, AppRoot as app_root, ImpostorHubComponent as impostor_hub, SMTPHostComponent as smtp_host, SMTPHostConfigurationComponent as smtp_host_configuration };
