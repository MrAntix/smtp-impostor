import { Component, h, Prop, Event, EventEmitter, Method } from '@stencil/core';
import {
  IHubSocketProvider, hubSocketProvider, IHubSocket,
  HubStatus, IHubMessage, hubStatusDisplay
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
  @Prop() url: string = `ws://localhost:62525/hub`;
  //@Prop() url: string = `wss://192.168.86.32:62526/hub`;
  @Prop({ mutable: true }) status: HubStatus = HubStatus.disconnected;

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
        this.setStatus(HubStatus.connected);
      };

      this.socket.onclose = (e: CloseEvent) => {
        this.logger.info('socket.onclose', { e });

        this.disconnectAsync();
        this.retryConnect();
      };

      this.socket.onerror = (e: any) => {
        this.logger.warn('socket.onerror', { e });

        this.setStatus(HubStatus.error);
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
      this.setStatus(HubStatus.error);
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

    this.setStatus(HubStatus.disconnected);
  }

  render() {
    return <div title={hubStatusDisplay(this.status)} class={`status status-${this.status}`}>
      <span class="buttons">
        <button title="stop/start worker"
          onClick={() => {
            if (this.status === HubStatus.disconnected)
              this.startupWorker.emit();
            else
              this.shutdownWorker.emit();
            this.status = HubStatus.working;
          }}
        ><span class="toggle"></span></button>
      </span>
    </div>;
  }

  setStatus(value: HubStatus) {
    this.status = value;
    this.statusChanged.emit(value);
  }

  @Event() statusChanged: EventEmitter<HubStatus>;
  @Event() messageReceived: EventEmitter<IHubMessage>;
  @Event() startupWorker: EventEmitter<void>;
  @Event() shutdownWorker: EventEmitter<void>;
}
