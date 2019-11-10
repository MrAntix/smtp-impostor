import { Component, h, State, Prop, Host } from '@stencil/core';
import '@stencil/redux';
import { Store } from '@stencil/redux';

import { IAppState, IAction, configureStore, DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA, Types } from '../redux';

import { HubStatus } from '../impostor-hub';
import {
  dispatch,
  initWorkerState,
  loadWorkerState,
  addHost,
  removeHost,
  startHost,
  stopHost,
  updateHost,
  toggleHostConfiguration,
  toggleHostMessages,
  searchHostMessages,
  deleteHostMessage
} from '../redux/state/actions';

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
  initWorkerState: typeof initWorkerState;
  loadWorkerState: typeof loadWorkerState;
  addHost: typeof addHost;
  removeHost: typeof removeHost;
  startHost: typeof startHost;
  stopHost: typeof stopHost;
  updateHost: typeof updateHost;
  toggleHostMessages: typeof toggleHostMessages;
  toggleHostConfiguration: typeof toggleHostConfiguration;
  searchHostMessages: typeof searchHostMessages;
  deleteHostMessage: typeof deleteHostMessage;

  async componentWillLoad() {
    this.store.setStore(configureStore({}, () => this.hubAction()));
    this.store.mapDispatchToProps(this, {
      dispatch,
      initWorkerState,
      loadWorkerState,
      addHost,
      removeHost,
      startHost,
      stopHost,
      updateHost,
      toggleHostMessages,
      toggleHostConfiguration,
      searchHostMessages,
      deleteHostMessage
    });
    this.store.mapStateToProps(this, (state: IAppState) => {
      if (state.worker.hosts) {
        state.worker.hosts.forEach(host => {
          if (!host.messages)
            this.searchHostMessages(host.id, DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA);
        });
      }

      return { state };
    });
  }

  hubAction() {
    return next => async (action: IAction) => {
      this.logger.debug('hubAction', { action });

      if (action.sendToHub) await this.hub.sendAsync(action);
      else {

        switch (action.type) {
          case Types.HOST_MESSAGE_RECEIVED:
            this.searchHostMessages(
              action.model.hostId,
              DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA);
            break;
        }

        await next(action);
      }
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

        {this.state.worker.hosts && (
          <div class="hosts">
            <ul>
              {this.state.worker.hosts.map(host => (
                <li key={host.id} class={{ "host": true, "host-open": host.showMessages }}>
                  <smtp-host
                    value={host}
                    showMessages={host.showMessages}
                    showConfiguration={host.showConfiguration}
                    onStartHost={e => this.startHost(e.detail.id)}
                    onStopHost={e => this.stopHost(e.detail.id)}
                    onUpdateHost={e => this.updateHost(e.detail)}
                    onToggleHostMessages={e => this.toggleHostMessages(host, e.detail.value)}
                    onSearchHostMessages={e => this.searchHostMessages(e.detail.hostId, e.detail.criteria)}
                    onDeleteHostMessage={e => this.deleteHostMessage(e.detail.hostId, e.detail.messageId)}
                  />
                  <div class="host-actions">
                    <button class="toggle-readonly" type="button"
                      onClick={() => this.toggleHostMessages(host)}>
                      <app-icon type="triangle" rotate={host.showMessages ? 0 : 180} />
                    </button>
                    {host.showMessages &&
                      <button
                        class="remove-host warning"
                        onClick={e => {
                          e.stopPropagation();
                          this.removeHost(host.id);
                        }}
                      >
                        <app-icon type="delete" />
                      </button>
                    }
                  </div>
                </li>
              ))}
            </ul>
            <div class="hosts-actions">
              <button class="add-host primary" onClick={() => this.addHost()}>
                <app-icon type="plus" />
              </button>
            </div>
          </div>
        )}

      </main>
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
        this.initWorkerState();
        break;
      case HubStatus.connected:
        this.loadWorkerState();
        break;
    }
  }
}
