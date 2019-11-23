import { Component, Prop, h, Host, Event, EventEmitter, Method } from '@stencil/core';
import { Frag } from '../dom';

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
        id: this.value.id,
        criteria: this.messagesSearchCriteria
      });
    }, debounce);
  }

  render() {
    if (this.value == null) return '(host not set)';

    return (
      <Host class={HostStatus[this.value.state].toLowerCase()}>
        <header>
          <div class="name" onDblClick={() => this.toggleHostMessages.emit({ id: this.value.id, value: !this.value.showMessages })}>
            {this.value.name}
            <small class="message-count">{this.value.messagesCount}</small>
          </div>
          <div class="actions">
            <button class="toggle-state" type="button" onClick={() => this.toggleState()}>
              <span>{this.value.state == HostStatus.Stopped ? "Start" : "Stop"}</span>
            </button>
          </div>
        </header>
        <div class="configuration">
          {this.showConfiguration && this.renderConfiguration()}
        </div>
        <div class="messages">
          {this.showMessages && this.renderMessages()}
        </div>
      </Host>
    );
  }

  renderConfiguration() {
    return <Frag>
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
          readOnly={!this.showConfiguration}
          onChange={(e: any) =>
            this.updateHost.emit({ id: this.value.id, name: e.target.value })
          }
        />
      </div>
      <div class="control name">
        <label>Max Messages</label>
        <input
          name="maxMessages"
          value={this.value.maxMessages}
          readOnly={!this.showConfiguration}
          onChange={(e: any) =>
            this.updateHost.emit({ id: this.value.id, maxMessages: e.target.value })
          }
        />
      </div>
      <div class="actions">
        <button class="ok primary"
          onClick={() => this.toggleHostConfiguration.emit({ id: this.value.id, value: !this.showMessages })}>
          <app-icon type="check" />
        </button>
        <button class="delete warning"
          onClick={() => confirm('Delete this host?') && this.removeHost.emit(this.value)}>
          <app-icon type="delete" /> Delete Host
        </button>
      </div>
    </Frag >;
  }

  renderMessages() {
    return <Frag>
      <div class="messages-toolbar">
        <app-input clear-button icon-type="search"
          value={this.messagesSearchCriteria.text}
          placeholder="search messages"
          onInputType={(e: any) => this.searchMessages({ text: e.detail }, 500)}
          onInputClear={() => this.searchMessages({ text: '' }, 0)}
        />
      </div>
      <ul class="messages-list">
        {this.value.messages && this.value.messages
          .map(message => <li class="message" data-id={message.id}
            onDblClick={() => this.openHostMessage.emit({ id: this.value.id, messageId: message.id })}>
            <div class="message-from">{message.from}</div>
            <div class="message-date" >
              {new Date(message.date).toLocaleString()}
              <button class="delete-message warning" type="button"
                onClick={() => this.deleteHostMessage.emit({
                  id: this.value.id,
                  messageId: message.id
                })}>
                <app-icon type="close"></app-icon>
              </button>
            </div>
            <div class="message-subject">{message.subject}</div>
          </li>)}
      </ul>
    </Frag>;
  }

  @Event() startHost: EventEmitter<IHost>;
  @Event() stopHost: EventEmitter<IHost>;
  @Event() updateHost: EventEmitter<IHostUpdate>;
  @Event() removeHost: EventEmitter<IHost>;
  @Event() toggleHostConfiguration: EventEmitter<{ id: string, value: boolean }>;
  @Event() toggleHostMessages: EventEmitter<{ id: string, value: boolean }>;
  @Event() searchHostMessages: EventEmitter<{ id: string, criteria: ISearchHostMessagesCriteria }>;
  @Event() deleteHostMessage: EventEmitter<{ id: string, messageId: string }>;
  @Event() openHostMessage: EventEmitter<{ id: string, messageId: string }>;
}
