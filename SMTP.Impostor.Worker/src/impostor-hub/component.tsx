import { Component, h, Prop, Event, EventEmitter, Method } from '@stencil/core';
import {
  IHubSocketProvider, hubSocketProvider, IHubSocket,
  HubState, IHubMessage
} from './model';

const RECONNECT_INIT = 200;
const RECONNECT_MAX = 15000;

@Component({
  tag: 'impostor-hub',
  styleUrl: 'component.css',
  shadow: true
})
export class ImpostorHubComponent {
  logger = globalThis.getLogger('ImpostorHubComponent');
  socket: IHubSocket;
  reconnectIn = 200;

  @Prop() socketProvider: IHubSocketProvider = hubSocketProvider;
  @Prop() url: string = `wss://${location.host}/hub`;
  @Prop() state: HubState = HubState.disconnected;

  @Method() async connectAsync(
    url?: string
  ) {
    if (url) this.url = url;

    this.logger.info('connecting', { url: this.url });

    try {
      if (this.socket) this.disconnectAsync();

      this.socket = this.socketProvider(this.url);

      this.socket.onopen = (e: any) => {
        this.logger.debug('socket.onopen', { e });

        this.reconnectIn = RECONNECT_INIT;
        this.setState(HubState.connected);
      };

      this.socket.onclose = (e: CloseEvent) => {
        this.logger.info('socket.onclose', { e });

        this.disconnectAsync();
        this.retryConnect();
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
      this.retryConnect();
    }
  }

  retryConnect() {
    this.logger.info('retryConnect', this.reconnectIn);

    setTimeout(() => this.connectAsync(), this.reconnectIn);

    this.reconnectIn *= 2
    if (this.reconnectIn > RECONNECT_MAX)
      this.reconnectIn = RECONNECT_MAX;
  }

  @Method() async sendAsync(message: IHubMessage) {
    if (!this.socket) throw new Error('cannot send, not connected');

    this.socket.send(
      JSON.stringify({
        type: message.type,
        data: message.model ? JSON.stringify(message.model) : undefined
      })
    );
  }

  @Method() async disconnectAsync() {
    this.logger.debug('disconnectAsync');
    if (this.socket.readyState < WebSocket.CLOSING) {
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
