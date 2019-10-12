import { Component, h, State, Prop } from '@stencil/core';
import "@stencil/redux";
import { Store } from "@stencil/redux";
import { Action } from 'redux';

import { HubStatus, IHubMessage } from '../impostor-hub';
import { IAppState, configureStore } from '../redux';
import * as fromStatus from '../redux/status';

@Component({
  tag: 'app-root',
  styleUrl: 'app-root.css',
  shadow: false
})
export class AppRoot {
  logger = globalThis.getLogger('AppRoot');
  hub: HTMLImpostorHubElement;

  @Prop({ context: "store" }) store: Store;

  @State() state: IAppState;

  act: (action: any) => void;

  async componentWillLoad() {
    this.store.setStore(configureStore({}));
    this.store.mapDispatchToProps(this, {
      act: (action: Action) => dispatch => dispatch(action)
    });
    this.store.mapStateToProps(this, (state: IAppState) => {
      return { state };
    });
  }

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

          <pre>{JSON.stringify(this.state, undefined, 2)}</pre>
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
      default:
        this.act({
          type: fromStatus.Types.STATUS,
          model: fromStatus.getInitialState()
        });
        break;
      case HubStatus.connected:
        await this.hub.sendAsync({
          type: 'GetStatus'
        });
        break;
    }
  }

  async handleHubMessageReceivedAsync(e: CustomEvent<IHubMessage>) {
    this.logger.debug('handleHubMessageReceivedAsync', { e });

    this.act(e.detail);
  }
}
