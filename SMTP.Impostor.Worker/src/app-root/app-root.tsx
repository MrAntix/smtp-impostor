import { Component, h, State } from '@stencil/core';
import { HubState, IHubMessage } from '../impostor-hub';


@Component({
  tag: 'app-root',
  styleUrl: 'app-root.css',
  shadow: false
})
export class AppRoot {
  logger = globalThis.getLogger('AppRoot');

  @State() state: any;
  hub: HTMLImpostorHubElement;

  render() {
    return (
      <div>
        <header>
          <h1>SMTP Impostor</h1>
          <impostor-hub ref={el => this.hub = el}
            onStateChanged={e => this.handleHubStateChangedAsync(e)}
            onMessageReceived={e => this.handleHubMessageReceivedAsync(e)}></impostor-hub>
        </header>

        <main>
          <stencil-router>
            <stencil-route-switch scrollTopOffset={0}>
              <stencil-route url='/' component='app-home' exact={true} />
            </stencil-route-switch>
          </stencil-router>

          <pre>{JSON.stringify(this.state, undefined, 2)}</pre>
        </main>
      </div>
    );
  }

  componentDidLoad() {
    this.hub.connectAsync()
  }

  async handleHubStateChangedAsync(e: CustomEvent<HubState>) {
    this.logger.debug('handleHubStateChangedAsync', { e });

    switch (e.detail) {
      case HubState.connected:
        await this.hub.sendAsync({
          type: 'GetState'
        });
        break;
    }
  }

  async handleHubMessageReceivedAsync(e: CustomEvent<IHubMessage>) {
    this.logger.debug('handleHubMessageReceivedAsync', { e });

    switch (e.detail.type) {
      case 'State':
        this.state = {
          ...e.detail.model
        };

        break;
    }
  }
}
