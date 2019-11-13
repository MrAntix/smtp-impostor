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
  deleteHostMessage,
  loadHostMessage
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
  loadHostMessage: typeof loadHostMessage;

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
      deleteHostMessage,
      loadHostMessage
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
    return (next: any) => async (action: IAction) => {
      this.logger.debug('hubAction', { action });

      if (action.sendToHub) await this.hub.sendAsync(action);
      else {

        switch (action.type) {
          case Types.HOST_MESSAGE_RECEIVED:
            this.searchHostMessages(
              action.model.hostId,
              DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA);
            break;

          case Types.HOST_MESSAGE:
            const file = new File(
              [action.model.content],
              `${action.model.id}.eml`,
              { type: 'message/rfc822' });

            const url = URL.createObjectURL(file);
            location.assign(url);
            URL.revokeObjectURL(url);

            break
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
                    onRemoveHost={e => this.removeHost(e.detail.id)}
                    onToggleHostConfiguration={e => this.toggleHostConfiguration(host, e.detail.value)}
                    onToggleHostMessages={e => this.toggleHostMessages(host, e.detail.value)}
                    onSearchHostMessages={e => this.searchHostMessages(e.detail.id, e.detail.criteria)}
                    onDeleteHostMessage={e => this.deleteHostMessage(e.detail.id, e.detail.messageId)}
                    onOpenHostMessage={e => this.loadHostMessage(e.detail.id, e.detail.messageId)}
                  />
                  <div class="host-actions">
                    <button class="toggle-readonly" type="button"
                      onClick={() => this.toggleHostMessages(host)}>
                      <app-icon type="triangle" scale={.65} rotate={host.showMessages ? 0 : 180} />
                    </button>
                    {host.showMessages &&
                      <button
                        class="remove-host"
                        onClick={() => this.toggleHostConfiguration(host)}
                      >
                        <app-icon type="cog" scale={1.667} />
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
