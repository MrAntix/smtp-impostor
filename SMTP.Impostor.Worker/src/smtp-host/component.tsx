import { Component, Prop, h, Host, Event, EventEmitter, Method } from '@stencil/core';
import { IHost, HostStatus, IHostUpdate } from '../redux';

@Component({
  tag: 'smtp-host',
  styleUrl: 'component.css',
  shadow: true
})
export class SMTPHostComponent {
  logger = globalThis.getLogger('SMTPHostComponent');

  @Prop() value: IHost = null;
  @Prop({ reflect: true }) readonly: boolean = true;

  @Method()
  async toggleReadonly(value?: boolean) {
    this.readonly = value == null ? !this.readonly : !!value;
  }

  @Method()
  async toggleState(start?: boolean) {
    start = start == null
      ? this.value.state !== HostStatus.Started
      : !!start;

    if (start) this.startHost.emit(this.value)
    else this.stopHost.emit(this.value)
  }

  render() {
    if (this.value == null) return '(host not set)';

    return (
      <Host class={HostStatus[this.value.state].toLowerCase()}>
        <div class="settings">
          <div class="control ip">
            <label>IP Address</label>
            <input
              name="ip"
              value={this.value.ip}
              onChange={(e: any) =>
                this.updateHost.emit({ id: this.value.id, ip: e.target.value })
              }
            />
          </div>
          <div class="control port">
            <label>Port</label>
            <input
              name="port"
              value={this.value.port}
              onChange={(e: any) =>
                this.updateHost.emit({ id: this.value.id, port: e.target.value })
              }
            />
          </div>
          <div class="control name">
            <label>Friendly Name</label>
            <input
              name="name"
              value={this.value.name}
              readOnly={this.readonly}
              onChange={(e: any) =>
                this.updateHost.emit({ id: this.value.id, name: e.target.value })
              }
            />
          </div>
        </div>
        <div class="actions">
          <button class="toggle-readonly" type="button" onClick={() => this.toggleReadonly()}>
            <app-icon type="triangle" rotate={this.readonly ? 180 : 0} />
          </button>
          <button class="toggle-state" type="button" onClick={() => this.toggleState()}>
            <span>{this.value.state == HostStatus.Stopped ? "Start" : "Stop"}</span>
          </button>
        </div>
      </Host>
    );
  }

  @Event() startHost: EventEmitter<IHost>;
  @Event() stopHost: EventEmitter<IHost>;
  @Event() updateHost: EventEmitter<IHostUpdate>;
}
