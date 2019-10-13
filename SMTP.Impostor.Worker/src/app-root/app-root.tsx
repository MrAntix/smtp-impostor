import { Component, h, State, Prop } from '@stencil/core';
import "@stencil/redux";
import { Store } from "@stencil/redux";

import { IAppState, IAction, configureStore } from '../redux';

import { HubStatus } from '../impostor-hub';
import { dispatch, initStatus, getStatus, addHost, removeHost, startHost, stopHost } from '../redux/status/actions';
import { HostStatus } from '../redux/status/model';

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

  dispatch: typeof dispatch;
  initStatus: typeof initStatus;
  getStatus: typeof getStatus;
  addHost: typeof addHost;
  removeHost: typeof removeHost;
  startHost: typeof startHost;
  stopHost: typeof stopHost;

  async componentWillLoad() {
    this.store.setStore(configureStore({}, () => this.hubAction()));
    this.store.mapDispatchToProps(this, {
      dispatch,
      initStatus, getStatus,
      addHost, removeHost, startHost, stopHost
    });
    this.store.mapStateToProps(this, (state: IAppState) => {
      return { state };
    });
  }

  hubAction() {
    return next => async (action: IAction) => {
      this.logger.debug('hubAction', { action });

      if (action.sendToHub)
        await this.hub.sendAsync(action);
      else next(action);
    }
  }

  render() {
    return (
      <div>
        <header>
          <h1>SMTP Impostor</h1>
        </header>

        <main>
          <stencil-router>
            <stencil-route-switch scrollTopOffset={0}>
              <stencil-route url='/' component='app-home' exact={true} />
            </stencil-route-switch>
          </stencil-router>

          {this.state.status.hosts &&
            <div class="hosts">
              <ul>
                {this.state.status.hosts.map(host => <li class="host">
                  <label>{host.name}</label>
                  <span>{HostStatus[host.state]}</span>
                  {host.state == HostStatus.Stopped
                    ? <button onClick={() => this.startHost(host.id)}>Start</button>
                    : <button onClick={() => this.stopHost(host.id)}>Stop</button>
                  }
                  <button onClick={() => this.removeHost(host.id)}>Remove</button>
                </li>)}
              </ul>
              <button onClick={() => this.addHost()}>Add Host</button>
            </div>
          }

          <pre>{JSON.stringify(this.state, undefined, 2)}</pre>
          <impostor-hub ref={el => this.hub = el}
            onStatusChanged={e => this.handleHubStatusChangedAsync(e)}
            onMessageReceived={e => this.dispatch(e.detail)}></impostor-hub>
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
        this.initStatus();
        break;
      case HubStatus.connected:
        this.getStatus();
        break;
    }
  }
}
