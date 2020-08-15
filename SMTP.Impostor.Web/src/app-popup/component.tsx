import { Component, Prop, h, Host, Event, EventEmitter, Method, Element } from "@stencil/core";

import { AppPopupPosition, AppPopupShift } from './model';

@Component({
  tag: "app-popup",
  styleUrl: "component.css",
  shadow: true
})
export class AppPopupComponent {
  @Element() element: HTMLElement;

  @Prop({ reflect: true, mutable: true }) isOpen: boolean = false;
  @Prop() modal: boolean = false;

  @Prop({ reflect: true, mutable: true }) position: AppPopupPosition;
  @Prop({ reflect: true, mutable: true }) shift?: AppPopupShift;

  @Method() async toggle(open?: boolean) {
    if (open == null) open = !this.isOpen;

    const e = this.toggled.emit(open);
    if (!e.defaultPrevented) this.isOpen = open;
  }

  render() {
    const hasHeader = this.element.querySelectorAll('[slot="popup-header"]').length > 0
    const hasFooter = this.element.querySelectorAll('[slot="popup-footer"]').length > 0

    return <Host
      onClick={e => this.handleClicked(e)}>
      <slot />
      <div class="overlay"></div>

      <div class="content">
        {hasHeader && <header class="header">
          <slot name="popup-header" />
        </header>}
        <div class="body">
          <slot name="popup-body" />
        </div>
        {hasFooter && <footer class="footer">
          <slot name="popup-footer" />
        </footer>
        }
      </div>
    </Host>
  }

  handleClicked(e: Event): void {
    e.stopPropagation();

    if (this.isOpen) {
      if (this.modal) return;

      const overlayElement = this.element.shadowRoot.querySelector('.overlay');
      if (!e.composedPath().some(et => et == overlayElement)) return;

      this.toggle(false);
      return;
    }

    this.toggle(true);
  }

  @Event() toggled: EventEmitter<boolean>;
}


