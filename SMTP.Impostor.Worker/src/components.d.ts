/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */


import { HTMLStencilElement, JSXBase } from '@stencil/core/internal';
import {
  AppIcons,
} from './app-icon/model';
import {
  AppIcons as AppIcons1,
} from './app-icon';
import {
  HubStatus,
  IHubMessage,
  IHubSocketProvider,
} from './impostor-hub/model';
import {
  IHost,
  IHostUpdate,
  ISearchHostMessagesCriteria,
} from './redux';

export namespace Components {
  interface AppIcon {
    'flipHorizontal': boolean;
    'flipVertical': boolean;
    'rotate': number;
    'scale': number;
    'type': AppIcons;
  }
  interface AppInput {
    'clearButton': boolean;
    'iconType'?: AppIcons;
    'placeholder': string;
    'value': string;
  }
  interface AppPopup {
    'isOpen': boolean;
    'modal': boolean;
    'toggle': (open?: boolean) => Promise<void>;
  }
  interface AppRoot {}
  interface ImpostorHub {
    'connectAsync': (url?: string) => Promise<void>;
    'disconnectAsync': () => Promise<void>;
    'sendAsync': (message: IHubMessage) => Promise<void>;
    'socketProvider': IHubSocketProvider;
    'status': HubStatus;
    'url': string;
  }
  interface SmtpHost {
    'searchMessages': (criteria: Partial<ISearchHostMessagesCriteria>, debounce?: number) => Promise<void>;
    'showConfiguration': boolean;
    'showMessages': boolean;
    'toggleState': (start?: boolean) => Promise<void>;
    'value': IHost;
  }
}

declare global {


  interface HTMLAppIconElement extends Components.AppIcon, HTMLStencilElement {}
  var HTMLAppIconElement: {
    prototype: HTMLAppIconElement;
    new (): HTMLAppIconElement;
  };

  interface HTMLAppInputElement extends Components.AppInput, HTMLStencilElement {}
  var HTMLAppInputElement: {
    prototype: HTMLAppInputElement;
    new (): HTMLAppInputElement;
  };

  interface HTMLAppPopupElement extends Components.AppPopup, HTMLStencilElement {}
  var HTMLAppPopupElement: {
    prototype: HTMLAppPopupElement;
    new (): HTMLAppPopupElement;
  };

  interface HTMLAppRootElement extends Components.AppRoot, HTMLStencilElement {}
  var HTMLAppRootElement: {
    prototype: HTMLAppRootElement;
    new (): HTMLAppRootElement;
  };

  interface HTMLImpostorHubElement extends Components.ImpostorHub, HTMLStencilElement {}
  var HTMLImpostorHubElement: {
    prototype: HTMLImpostorHubElement;
    new (): HTMLImpostorHubElement;
  };

  interface HTMLSmtpHostElement extends Components.SmtpHost, HTMLStencilElement {}
  var HTMLSmtpHostElement: {
    prototype: HTMLSmtpHostElement;
    new (): HTMLSmtpHostElement;
  };
  interface HTMLElementTagNameMap {
    'app-icon': HTMLAppIconElement;
    'app-input': HTMLAppInputElement;
    'app-popup': HTMLAppPopupElement;
    'app-root': HTMLAppRootElement;
    'impostor-hub': HTMLImpostorHubElement;
    'smtp-host': HTMLSmtpHostElement;
  }
}

declare namespace LocalJSX {
  interface AppIcon {
    'flipHorizontal'?: boolean;
    'flipVertical'?: boolean;
    'rotate'?: number;
    'scale'?: number;
    'type'?: AppIcons;
  }
  interface AppInput {
    'clearButton'?: boolean;
    'iconType'?: AppIcons;
    'onInputChange'?: (event: CustomEvent<string>) => void;
    'onInputClear'?: (event: CustomEvent<void>) => void;
    'onInputType'?: (event: CustomEvent<string>) => void;
    'placeholder'?: string;
    'value'?: string;
  }
  interface AppPopup {
    'isOpen'?: boolean;
    'modal'?: boolean;
    'onToggled'?: (event: CustomEvent<boolean>) => void;
  }
  interface AppRoot {}
  interface ImpostorHub {
    'onMessageReceived'?: (event: CustomEvent<IHubMessage>) => void;
    'onShutdownWorker'?: (event: CustomEvent<void>) => void;
    'onStatusChanged'?: (event: CustomEvent<HubStatus>) => void;
    'socketProvider'?: IHubSocketProvider;
    'status'?: HubStatus;
    'url'?: string;
  }
  interface SmtpHost {
    'onDeleteHostMessage'?: (event: CustomEvent<{ id: string, messageId: string }>) => void;
    'onOpenHostMessage'?: (event: CustomEvent<{ id: string, messageId: string }>) => void;
    'onRemoveHost'?: (event: CustomEvent<IHost>) => void;
    'onSearchHostMessages'?: (event: CustomEvent<{ id: string, criteria: ISearchHostMessagesCriteria }>) => void;
    'onStartHost'?: (event: CustomEvent<IHost>) => void;
    'onStopHost'?: (event: CustomEvent<IHost>) => void;
    'onToggleHostConfiguration'?: (event: CustomEvent<{ id: string, value: boolean }>) => void;
    'onToggleHostMessages'?: (event: CustomEvent<{ id: string, value: boolean }>) => void;
    'onUpdateHost'?: (event: CustomEvent<IHostUpdate>) => void;
    'showConfiguration'?: boolean;
    'showMessages'?: boolean;
    'value'?: IHost;
  }

  interface IntrinsicElements {
    'app-icon': AppIcon;
    'app-input': AppInput;
    'app-popup': AppPopup;
    'app-root': AppRoot;
    'impostor-hub': ImpostorHub;
    'smtp-host': SmtpHost;
  }
}

export { LocalJSX as JSX };


declare module "@stencil/core" {
  export namespace JSX {
    interface IntrinsicElements {
      'app-icon': LocalJSX.AppIcon & JSXBase.HTMLAttributes<HTMLAppIconElement>;
      'app-input': LocalJSX.AppInput & JSXBase.HTMLAttributes<HTMLAppInputElement>;
      'app-popup': LocalJSX.AppPopup & JSXBase.HTMLAttributes<HTMLAppPopupElement>;
      'app-root': LocalJSX.AppRoot & JSXBase.HTMLAttributes<HTMLAppRootElement>;
      'impostor-hub': LocalJSX.ImpostorHub & JSXBase.HTMLAttributes<HTMLImpostorHubElement>;
      'smtp-host': LocalJSX.SmtpHost & JSXBase.HTMLAttributes<HTMLSmtpHostElement>;
    }
  }
}


