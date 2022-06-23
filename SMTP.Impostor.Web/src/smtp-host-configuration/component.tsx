import { Component, Prop, h, Host, Event, EventEmitter } from '@stencil/core';

import {
  IHost, IHostUpdate
} from '../redux';

@Component({
  tag: 'smtp-host-configuration',
  styleUrl: 'component.css',
  shadow: { delegatesFocus: true }
})
export class SMTPHostConfigurationComponent {
  logger = globalThis.getLogger('SMTPHostConfigurationComponent');

  @Prop() value: IHost = null;

  render() {
    if (!this.value) return '(host not set)';

    return <Host>
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
          onChange={(e: any) =>
            this.updateHost.emit({ id: this.value.id, name: e.target.value })
          }
        />
      </div>
      <div class="control name">
        <label>Max Messages</label>
        <input
          name="maxMessages"
          value={this.value.maxMessages || ''}
          onChange={(e: any) =>
            this.updateHost.emit({ id: this.value.id, maxMessages: e.target.value || 0 })
          }
        />
      </div>
    </Host>;
  }

  @Event() updateHost: EventEmitter<IHostUpdate>;
}
