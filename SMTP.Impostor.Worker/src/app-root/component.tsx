import { Component, h, State, Prop, Host } from '@stencil/core';
import '@stencil/redux';
import { Store } from '@stencil/redux';

import { IAppState, IAction, configureStore } from '../redux';

import { HubStatus } from '../impostor-hub';
import {
  dispatch,
  initStatus,
  getStatus,
  addHost,
  removeHost,
  startHost,
  stopHost,
  updateHost,
  toggleHostConfiguration,
  toggleHostMessages
} from '../redux/status/actions';

@Component({
  tag: 'app-root',
  styleUrl: 'component.css',
  shadow: true
})
export class AppRoot {
  logger = globalThis.getLogger('AppRoot');
  hub: HTMLImpostorHubElement;

  @Prop({ context: 'store' }) store: Store;

  @State() state: IAppState;

  dispatch: typeof dispatch;
  initStatus: typeof initStatus;
  getStatus: typeof getStatus;
  addHost: typeof addHost;
  removeHost: typeof removeHost;
  startHost: typeof startHost;
  stopHost: typeof stopHost;
  updateHost: typeof updateHost;
  toggleHostMessages: typeof toggleHostMessages;
  toggleHostConfiguration: typeof toggleHostConfiguration;

  async componentWillLoad() {
    this.store.setStore(configureStore({}, () => this.hubAction()));
    this.store.mapDispatchToProps(this, {
      dispatch,
      initStatus,
      getStatus,
      addHost,
      removeHost,
      startHost,
      stopHost,
      updateHost,
      toggleHostMessages,
      toggleHostConfiguration
    });
    this.store.mapStateToProps(this, (state: IAppState) => {
      return { state };
    });
  }

  hubAction() {
    return next => async (action: IAction) => {
      this.logger.debug('hubAction', { action });

      if (action.sendToHub) await this.hub.sendAsync(action);
      else next(action);
    };
  }

  render() {
    return <Host>
      <main>
        <stencil-router>
          <stencil-route-switch scrollTopOffset={0}>
            <stencil-route url="/" component="app-home" exact={true} />
          </stencil-route-switch>
        </stencil-router>

        {this.state.status.hosts && (
          <div class="hosts">
            <ul>
              {this.state.status.hosts.map(host => (
                <li key={host.id} class="host"
                  onClick={() => this.toggleHostMessages(host)}>
                  <smtp-host
                    value={host}
                    showMessages={host.showMessages}
                    showConfiguration={host.showConfiguration}
                    onStartHost={e => this.startHost(e.detail.id)}
                    onStopHost={e => this.stopHost(e.detail.id)}
                    onUpdateHost={e => this.updateHost(e.detail)}
                  />
                  <button class="toggle-readonly" type="button">
                    <app-icon type="triangle" rotate={host.showMessages ? 0 : 180} />
                  </button>
                  <button
                    class="remove-host warning"
                    onClick={e => {
                      e.stopPropagation()
                    }}
                  >
                    <app-icon type="cog" />
                  </button>
                </li>
              ))}
            </ul>
            <button class="add-host primary" onClick={() => this.addHost()}>
              <app-icon type="plus" />
            </button>
          </div>
        )}

      </main>
      <pre>{JSON.stringify(this.state, undefined, 2)}</pre>
      <impostor-hub
        ref={el => (this.hub = el)}
        onStatusChanged={e => this.handleHubStatusChangedAsync(e)}
        onMessageReceived={e => this.dispatch(e.detail)}
      ></impostor-hub>
    </Host>;
  }

  componentDidLoad() {
    this.hub.connectAsync();
  }

  async handleHubStatusChangedAsync(e: CustomEvent<HubStatus>) {
    this.logger.debug('handleHubStatusChangedAsync', { e });

    switch (e.detail) {
      default:
        this.initStatus();
        break;
      case HubStatus.connected:
        this.getStatus();
        break;
    }
  }
}
