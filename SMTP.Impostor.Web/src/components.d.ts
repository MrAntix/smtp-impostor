/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { AppIcons } from "./app-icon/model";
import { AppIcons as AppIcons1 } from "./app-icon";
import { AppPopupPosition, AppPopupShift } from "./app-popup/model";
import { HubStatus, IHubMessage, IHubSocketProvider } from "./impostor-hub/model";
import { IHost, IHostUpdate, ISearchHostMessagesCriteria } from "./redux";
export { AppIcons } from "./app-icon/model";
export { AppIcons as AppIcons1 } from "./app-icon";
export { AppPopupPosition, AppPopupShift } from "./app-popup/model";
export { HubStatus, IHubMessage, IHubSocketProvider } from "./impostor-hub/model";
export { IHost, IHostUpdate, ISearchHostMessagesCriteria } from "./redux";
export namespace Components {
    interface AppIcon {
        "flipHorizontal": boolean;
        "flipVertical": boolean;
        "rotate": number;
        "scale": number;
        "type": AppIcons;
    }
    interface AppInput {
        "clearButton": boolean;
        "iconType"?: AppIcons1;
        "placeholder": string;
        "value": string;
    }
    interface AppPopup {
        "isOpen": boolean;
        "modal": boolean;
        "position": AppPopupPosition;
        "shift"?: AppPopupShift;
        "toggle": (open?: boolean) => Promise<void>;
    }
    interface AppRoot {
    }
    interface ImpostorHub {
        "connectAsync": (url?: string) => Promise<void>;
        "disconnectAsync": () => Promise<void>;
        "sendAsync": (message: IHubMessage) => Promise<void>;
        "socketProvider": IHubSocketProvider;
        "status": HubStatus;
        "url": string;
    }
    interface SmtpHost {
        "searchMessages": (criteria?: Partial<ISearchHostMessagesCriteria>, debounce?: number) => Promise<void>;
        "showConfiguration": boolean;
        "showMessages": boolean;
        "toggleState": (start?: boolean) => Promise<void>;
        "value": IHost;
    }
    interface SmtpHostConfiguration {
        "value": IHost;
    }
}
export interface AppInputCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLAppInputElement;
}
export interface AppPopupCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLAppPopupElement;
}
export interface ImpostorHubCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLImpostorHubElement;
}
export interface SmtpHostCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLSmtpHostElement;
}
export interface SmtpHostConfigurationCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLSmtpHostConfigurationElement;
}
declare global {
    interface HTMLAppIconElement extends Components.AppIcon, HTMLStencilElement {
    }
    var HTMLAppIconElement: {
        prototype: HTMLAppIconElement;
        new (): HTMLAppIconElement;
    };
    interface HTMLAppInputElementEventMap {
        "inputType": string;
        "inputChange": string;
        "inputClear": void;
    }
    interface HTMLAppInputElement extends Components.AppInput, HTMLStencilElement {
        addEventListener<K extends keyof HTMLAppInputElementEventMap>(type: K, listener: (this: HTMLAppInputElement, ev: AppInputCustomEvent<HTMLAppInputElementEventMap[K]>) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void;
        removeEventListener<K extends keyof HTMLAppInputElementEventMap>(type: K, listener: (this: HTMLAppInputElement, ev: AppInputCustomEvent<HTMLAppInputElementEventMap[K]>) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | EventListenerOptions): void;
    }
    var HTMLAppInputElement: {
        prototype: HTMLAppInputElement;
        new (): HTMLAppInputElement;
    };
    interface HTMLAppPopupElementEventMap {
        "toggled": boolean;
    }
    interface HTMLAppPopupElement extends Components.AppPopup, HTMLStencilElement {
        addEventListener<K extends keyof HTMLAppPopupElementEventMap>(type: K, listener: (this: HTMLAppPopupElement, ev: AppPopupCustomEvent<HTMLAppPopupElementEventMap[K]>) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void;
        removeEventListener<K extends keyof HTMLAppPopupElementEventMap>(type: K, listener: (this: HTMLAppPopupElement, ev: AppPopupCustomEvent<HTMLAppPopupElementEventMap[K]>) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | EventListenerOptions): void;
    }
    var HTMLAppPopupElement: {
        prototype: HTMLAppPopupElement;
        new (): HTMLAppPopupElement;
    };
    interface HTMLAppRootElement extends Components.AppRoot, HTMLStencilElement {
    }
    var HTMLAppRootElement: {
        prototype: HTMLAppRootElement;
        new (): HTMLAppRootElement;
    };
    interface HTMLImpostorHubElementEventMap {
        "statusChanged": HubStatus;
        "messageReceived": IHubMessage;
        "startupWorker": void;
        "shutdownWorker": void;
    }
    interface HTMLImpostorHubElement extends Components.ImpostorHub, HTMLStencilElement {
        addEventListener<K extends keyof HTMLImpostorHubElementEventMap>(type: K, listener: (this: HTMLImpostorHubElement, ev: ImpostorHubCustomEvent<HTMLImpostorHubElementEventMap[K]>) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void;
        removeEventListener<K extends keyof HTMLImpostorHubElementEventMap>(type: K, listener: (this: HTMLImpostorHubElement, ev: ImpostorHubCustomEvent<HTMLImpostorHubElementEventMap[K]>) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | EventListenerOptions): void;
    }
    var HTMLImpostorHubElement: {
        prototype: HTMLImpostorHubElement;
        new (): HTMLImpostorHubElement;
    };
    interface HTMLSmtpHostElementEventMap {
        "startHost": IHost;
        "stopHost": IHost;
        "openHost": { id: string };
        "searchHostMessages": { id: string, criteria: ISearchHostMessagesCriteria };
        "deleteHostMessage": { id: string, messageId: string };
        "openHostMessage": { id: string, messageId: string };
    }
    interface HTMLSmtpHostElement extends Components.SmtpHost, HTMLStencilElement {
        addEventListener<K extends keyof HTMLSmtpHostElementEventMap>(type: K, listener: (this: HTMLSmtpHostElement, ev: SmtpHostCustomEvent<HTMLSmtpHostElementEventMap[K]>) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void;
        removeEventListener<K extends keyof HTMLSmtpHostElementEventMap>(type: K, listener: (this: HTMLSmtpHostElement, ev: SmtpHostCustomEvent<HTMLSmtpHostElementEventMap[K]>) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | EventListenerOptions): void;
    }
    var HTMLSmtpHostElement: {
        prototype: HTMLSmtpHostElement;
        new (): HTMLSmtpHostElement;
    };
    interface HTMLSmtpHostConfigurationElementEventMap {
        "updateHost": IHostUpdate;
    }
    interface HTMLSmtpHostConfigurationElement extends Components.SmtpHostConfiguration, HTMLStencilElement {
        addEventListener<K extends keyof HTMLSmtpHostConfigurationElementEventMap>(type: K, listener: (this: HTMLSmtpHostConfigurationElement, ev: SmtpHostConfigurationCustomEvent<HTMLSmtpHostConfigurationElementEventMap[K]>) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | AddEventListenerOptions): void;
        addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void;
        removeEventListener<K extends keyof HTMLSmtpHostConfigurationElementEventMap>(type: K, listener: (this: HTMLSmtpHostConfigurationElement, ev: SmtpHostConfigurationCustomEvent<HTMLSmtpHostConfigurationElementEventMap[K]>) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener<K extends keyof HTMLElementEventMap>(type: K, listener: (this: HTMLElement, ev: HTMLElementEventMap[K]) => any, options?: boolean | EventListenerOptions): void;
        removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | EventListenerOptions): void;
    }
    var HTMLSmtpHostConfigurationElement: {
        prototype: HTMLSmtpHostConfigurationElement;
        new (): HTMLSmtpHostConfigurationElement;
    };
    interface HTMLElementTagNameMap {
        "app-icon": HTMLAppIconElement;
        "app-input": HTMLAppInputElement;
        "app-popup": HTMLAppPopupElement;
        "app-root": HTMLAppRootElement;
        "impostor-hub": HTMLImpostorHubElement;
        "smtp-host": HTMLSmtpHostElement;
        "smtp-host-configuration": HTMLSmtpHostConfigurationElement;
    }
}
declare namespace LocalJSX {
    interface AppIcon {
        "flipHorizontal"?: boolean;
        "flipVertical"?: boolean;
        "rotate"?: number;
        "scale"?: number;
        "type"?: AppIcons;
    }
    interface AppInput {
        "clearButton"?: boolean;
        "iconType"?: AppIcons1;
        "onInputChange"?: (event: AppInputCustomEvent<string>) => void;
        "onInputClear"?: (event: AppInputCustomEvent<void>) => void;
        "onInputType"?: (event: AppInputCustomEvent<string>) => void;
        "placeholder"?: string;
        "value"?: string;
    }
    interface AppPopup {
        "isOpen"?: boolean;
        "modal"?: boolean;
        "onToggled"?: (event: AppPopupCustomEvent<boolean>) => void;
        "position"?: AppPopupPosition;
        "shift"?: AppPopupShift;
    }
    interface AppRoot {
    }
    interface ImpostorHub {
        "onMessageReceived"?: (event: ImpostorHubCustomEvent<IHubMessage>) => void;
        "onShutdownWorker"?: (event: ImpostorHubCustomEvent<void>) => void;
        "onStartupWorker"?: (event: ImpostorHubCustomEvent<void>) => void;
        "onStatusChanged"?: (event: ImpostorHubCustomEvent<HubStatus>) => void;
        "socketProvider"?: IHubSocketProvider;
        "status"?: HubStatus;
        "url"?: string;
    }
    interface SmtpHost {
        "onDeleteHostMessage"?: (event: SmtpHostCustomEvent<{ id: string, messageId: string }>) => void;
        "onOpenHost"?: (event: SmtpHostCustomEvent<{ id: string }>) => void;
        "onOpenHostMessage"?: (event: SmtpHostCustomEvent<{ id: string, messageId: string }>) => void;
        "onSearchHostMessages"?: (event: SmtpHostCustomEvent<{ id: string, criteria: ISearchHostMessagesCriteria }>) => void;
        "onStartHost"?: (event: SmtpHostCustomEvent<IHost>) => void;
        "onStopHost"?: (event: SmtpHostCustomEvent<IHost>) => void;
        "showConfiguration"?: boolean;
        "showMessages"?: boolean;
        "value"?: IHost;
    }
    interface SmtpHostConfiguration {
        "onUpdateHost"?: (event: SmtpHostConfigurationCustomEvent<IHostUpdate>) => void;
        "value"?: IHost;
    }
    interface IntrinsicElements {
        "app-icon": AppIcon;
        "app-input": AppInput;
        "app-popup": AppPopup;
        "app-root": AppRoot;
        "impostor-hub": ImpostorHub;
        "smtp-host": SmtpHost;
        "smtp-host-configuration": SmtpHostConfiguration;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "app-icon": LocalJSX.AppIcon & JSXBase.HTMLAttributes<HTMLAppIconElement>;
            "app-input": LocalJSX.AppInput & JSXBase.HTMLAttributes<HTMLAppInputElement>;
            "app-popup": LocalJSX.AppPopup & JSXBase.HTMLAttributes<HTMLAppPopupElement>;
            "app-root": LocalJSX.AppRoot & JSXBase.HTMLAttributes<HTMLAppRootElement>;
            "impostor-hub": LocalJSX.ImpostorHub & JSXBase.HTMLAttributes<HTMLImpostorHubElement>;
            "smtp-host": LocalJSX.SmtpHost & JSXBase.HTMLAttributes<HTMLSmtpHostElement>;
            "smtp-host-configuration": LocalJSX.SmtpHostConfiguration & JSXBase.HTMLAttributes<HTMLSmtpHostConfigurationElement>;
        }
    }
}
