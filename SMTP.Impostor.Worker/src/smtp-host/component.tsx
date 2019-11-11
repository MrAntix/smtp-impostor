import { Component, Prop, h, Host, Event, EventEmitter, Method } from '@stencil/core';
import {
  IHost, HostStatus, IHostUpdate,
  ISearchHostMessagesCriteria, DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA
} from '../redux';

@Component({
  tag: 'smtp-host',
  styleUrl: 'component.css',
  shadow: true
})
export class SMTPHostComponent {
  logger = globalThis.getLogger('SMTPHostComponent');

  @Prop() value: IHost = null;
  @Prop({ reflect: true }) showMessages: boolean = false;
  @Prop({ reflect: true }) showConfiguration: boolean = false;

  @Method()
  async toggleState(start?: boolean) {
    start = start == null
      ? this.value.state !== HostStatus.Started
      : !!start;

    if (start) this.startHost.emit(this.value)
    else this.stopHost.emit(this.value)
  }

  messagesSearchCriteria = DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA;
  messagesSearchTimer: any;

  @Method()
  async searchMessages(
    criteria: Partial<ISearchHostMessagesCriteria>,
    debounce: number = 0) {

    this.messagesSearchCriteria = {
      ...this.messagesSearchCriteria,
      ...criteria
    };

    if (this.messagesSearchTimer) clearTimeout(this.messagesSearchTimer);
    this.messagesSearchTimer = setTimeout(() => {
      console.log('searchMessages', this.messagesSearchCriteria);

      this.searchHostMessages.emit({
        hostId: this.value.id,
        criteria: this.messagesSearchCriteria
      });
    }, debounce);
  }

  render() {
    if (this.value == null) return '(host not set)';

    return (
      <Host class={HostStatus[this.value.state].toLowerCase()}>
        <header>
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
              {this.value.showConfiguration
                ? <input
                  name="name"
                  value={this.value.name}
                  readOnly={!this.showConfiguration}
                  onChange={(e: any) =>
                    this.updateHost.emit({ id: this.value.id, name: e.target.value })
                  }
                />
                : <span onDblClick={() => this.toggleHostMessages.emit({ hostId: this.value.id, value: !this.value.showMessages })}>{this.value.name}</span>
              }
              <small>{this.value.messagesCount}</small>
            </div>
          </div>
          <div class="actions">
            <button class="toggle-state" type="button" onClick={() => this.toggleState()}>
              <span>{this.value.state == HostStatus.Stopped ? "Start" : "Stop"}</span>
            </button>
          </div>
        </header>
        {this.value.showMessages && this.renderMessages()}
      </Host>
    );
  }

  renderMessages() {
    return <main class="messages">
      <div class="messages-toolbar">
        <input onInput={(e: any) =>
          this.searchMessages({ text: e.target.value }, 500)} />
        <app-icon type="search" />
      </div>
      <ul class="messages-list">
        {this.value.messages && this.value.messages
          .map(message => <li class="message" data-id={message.id}>
            <div class="message-from">{message.from}</div>
            <div class="message-date" >
              {new Date(message.date).toLocaleString()}
              <button class="delete-message warning" type="button"
                onClick={() => this.deleteHostMessage.emit({
                  hostId: this.value.id,
                  messageId: message.id
                })}>
                <app-icon type="close"></app-icon>
              </button>
            </div>
            <div class="message-subject">{message.subject}</div>
          </li>)}
      </ul>
    </main>
  }

  @Event() startHost: EventEmitter<IHost>;
  @Event() stopHost: EventEmitter<IHost>;
  @Event() updateHost: EventEmitter<IHostUpdate>;
  @Event() toggleHostMessages: EventEmitter<{ hostId: string, value: boolean }>;
  @Event() searchHostMessages: EventEmitter<{ hostId: string, criteria: ISearchHostMessagesCriteria }>;
  @Event() deleteHostMessage: EventEmitter<{ hostId: string, messageId: string }>;
}
