import { Component, h, State } from '@stencil/core';
import { HubStatus, IHubMessage } from '../impostor-hub';


@Component({
  tag: 'app-root',
  styleUrl: 'app-root.css',
  shadow: false
})
export class AppRoot {
  logger = globalThis.getLogger('AppRoot');

  @State() status: any;
  hub: HTMLImpostorHubElement;

  render() {
    return (
      <div>
        <header>
          <h1>SMTP Impostor</h1>
          <impostor-hub ref={el => this.hub = el}
            onStatusChanged={e => this.handleHubStatusChangedAsync(e)}
            onMessageReceived={e => this.handleHubMessageReceivedAsync(e)}></impostor-hub>
        </header>

        <main>
          <stencil-router>
            <stencil-route-switch scrollTopOffset={0}>
              <stencil-route url='/' component='app-home' exact={true} />
            </stencil-route-switch>
          </stencil-router>

          <pre>{JSON.stringify(this.status, undefined, 2)}</pre>
        </main>
      </div>
    );
  }

  componentDidLoad() {
    this.hub.connectAsync()
  }

  async handleHubStatusChangedAsync(e: CustomEvent<HubStatus>) {
    this.logger.debug('handleHubStatusChangedAsync', { e });

    switch (e.detail) {
      case HubStatus.connected:
        await this.hub.sendAsync({
          type: 'GetStatus'
        });
        break;
    }
  }

  async handleHubMessageReceivedAsync(e: CustomEvent<IHubMessage>) {
    this.logger.debug('handleHubMessageReceivedAsync', { e });

    switch (e.detail.type) {
      case 'Status':
        this.status = {
          ...e.detail.model
        };

        break;
    }
  }
}
