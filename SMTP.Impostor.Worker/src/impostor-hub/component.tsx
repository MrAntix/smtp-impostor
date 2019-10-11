import { Component, h, Prop, Event, EventEmitter, Method } from '@stencil/core';
import {
  IHubSocketProvider, hubSocketProvider, IHubSocket,
  HubState, IHubMessage
} from './model';

@Component({
  tag: 'impostor-hub',
  styleUrl: 'component.css',
  shadow: true
})
export class ImpostorHubComponent {
  logger = globalThis.getLogger('ImpostorHubComponent');
  socket: IHubSocket;

  @Prop() socketProvider: IHubSocketProvider = hubSocketProvider;
  @Prop() url: string = `wss://${location.host}/hub`;
  @Prop() state: HubState = HubState.disconnected;

  @Method() async connectAsync(
    url?: string
  ) {
    if (url) this.url = url;

    this.logger.info('connect', { url: this.url });

    try {
      if (this.socket) this.disconnectAsync();

      this.socket = this.socketProvider(this.url);

      this.socket.onopen = (e: any) => {
        this.logger.debug('socket.onopen', { e });

        this.setState(HubState.connected);
      };

      this.socket.onclose = (e: CloseEvent) => {
        this.logger.info('socket.onclose', { e });

        this.disconnectAsync();
      };

      this.socket.onerror = (e: any) => {
        this.logger.warn('socket.onerror', { e });

        this.setState(HubState.error);
      };

      this.socket.onmessage = (e: MessageEvent) => {
        this.logger.debug('socket.onmessage', { e });

        const message = JSON.parse(e.data);

        this.messageReceived.emit({
          type: message.type,
          model: message.data && JSON.parse(message.data)
        })
      };

    } catch (err) {
      this.logger.error('connect', err);
      this.setState(HubState.error);
    }
  }

  @Method() async sendAsync(message: IHubMessage) {
    if(!this.socket) throw new Error('cannot send, not connected');

    this.socket.send(
      JSON.stringify({
        type: message.type,
        data: message.model ? JSON.stringify(message.model) : undefined
      })
    );
  }

  @Method() async disconnectAsync() {
    this.logger.debug('disconnectAsync');
    if (this.socket.readyState === WebSocket.OPEN) {
      this.logger.debug('disconnectAsync socket.close');
      this.socket.close()
    }
    this.socket = null;

    this.setState(HubState.disconnected);
  }

  render() {
    return <div>State {HubState[this.state]}</div>;
  }

  setState(state: HubState) {
    this.state = state;
    this.stateChanged.emit(state);
  }

  @Event() stateChanged: EventEmitter<HubState>;
  @Event() messageReceived: EventEmitter<IHubMessage>;
}
