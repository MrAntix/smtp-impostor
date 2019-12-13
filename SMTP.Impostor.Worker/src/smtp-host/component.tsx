import { Component, Prop, h, Host, Event, EventEmitter, Method } from '@stencil/core';
import { Frag } from '../dom';

import {
  IHost, HostStatus,
  ISearchHostMessagesCriteria, DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA, hostIsRunning
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
      ? !hostIsRunning(this.value)
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
          <div class="name" onDblClick={() => this.openHost.emit({ id: this.value.id })}>
            {this.value.name}
            <small class="message-count">{this.value.messagesCount}</small>
          </div>
          <div class="actions">
            <button class="toggle-state" type="button" onClick={() => this.toggleState()}>
              <span>{hostIsRunning(this.value) ? "Stop" : "Start"}</span>
            </button>
          </div>
        </header>
        <div class="messages">
          {this.showMessages && this.renderMessages()}
        </div>
      </Host>
    );
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
              <button class="delete-message danger" type="button"
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
  @Event() openHost: EventEmitter<{ id: string }>;
  @Event() searchHostMessages: EventEmitter<{ id: string, criteria: ISearchHostMessagesCriteria }>;
  @Event() deleteHostMessage: EventEmitter<{ id: string, messageId: string }>;
  @Event() openHostMessage: EventEmitter<{ id: string, messageId: string }>;
}
