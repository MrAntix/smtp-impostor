import { Component, Prop, h, Host, Event, EventEmitter, Method } from "@stencil/core";

import { AppPopupPosition, AppPopupShift } from './model';

@Component({
  tag: "app-popup",
  styleUrl: "component.css",
  shadow: true
})
export class AppPopupComponent {

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

    return <Host
      onClick={e => this.handleClicked(e)}>
      <slot />
      <div class="overlay"
        onClick={e => this.handleOverlayClicked(e)}></div>

      <div class="content"
        onClick={e => this.handleContentClicked(e)}>
        <slot name="popup-content" />
      </div>
    </Host>
  }

  handleClicked(e: Event): void {
    e.stopPropagation();

    this.toggle();
  }

  handleOverlayClicked(e: Event): void {
    e.stopPropagation();

    if (!this.modal) this.toggle(false);
  }

  handleContentClicked(e: Event): void {
    e.stopPropagation();
  }

  @Event() toggled: EventEmitter<boolean>;
}


