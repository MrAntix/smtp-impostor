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
  openHost,
  toggleHostConfiguration,
  searchHostMessages,
  deleteHostMessage,
  loadHostMessage,
  shutdownWorker
} from '../redux/state/actions';
import { newId } from '../global';

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
  openHost: typeof openHost;
  toggleHostConfiguration: typeof toggleHostConfiguration;
  searchHostMessages: typeof searchHostMessages;
  deleteHostMessage: typeof deleteHostMessage;
  loadHostMessage: typeof loadHostMessage;
  shutdownWorker: typeof shutdownWorker;
  newHostId: string;

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
      openHost,
      updateHost,
      toggleHostConfiguration,
      searchHostMessages,
      deleteHostMessage,
      loadHostMessage,
      shutdownWorker
    });
    this.store.mapStateToProps(this, (state: IAppState) => {

      return { state };
    });
  }

  hubAction() {
    return (next: any) => async (action: IAction) => {
      this.logger.debug('hubAction', action.type, { action });

      if (action.sendToHub) await this.hub.sendAsync(action);
      else {

        switch (action.type) {
          case Types.WORKER_STATE:

            if (this.newHostId) {
              const hostId = this.newHostId;
              requestAnimationFrame(() => {
                this.openHost(hostId);
                this.toggleHostConfiguration(hostId, true);
              });
              this.newHostId = null;
            }

            break;

          case Types.HOST_MESSAGE_RECEIVED:
            if (this.state.worker.openHostId === action.model.hostId)
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
                <li key={host.id} class={{ "host": true, "host-open": this.state.worker.openHostId === host.id }}>
                  <smtp-host
                    value={host}
                    showMessages={this.state.worker.openHostId === host.id}
                    onStartHost={e => this.startHost(e.detail.id)}
                    onStopHost={e => this.stopHost(e.detail.id)}
                    onOpenHost={() => this.openHost(this.state.worker.openHostId === host.id ? null : host.id)}
                    onSearchHostMessages={e => this.searchHostMessages(e.detail.id, e.detail.criteria)}
                    onDeleteHostMessage={e => this.deleteHostMessage(e.detail.id, e.detail.messageId)}
                    onOpenHostMessage={e => this.loadHostMessage(e.detail.id, e.detail.messageId)}
                  />
                  <div class="host-actions">
                    <button class="toggle-readonly" type="button"
                      onClick={() => this.openHost(this.state.worker.openHostId === host.id ? null : host.id)}>
                      <app-icon type="triangle" scale={.65} rotate={this.state.worker.openHostId === host.id ? 0 : 180} />
                    </button>
                    {host.showConfiguration}
                    {this.state.worker.openHostId === host.id &&
                      <app-popup position="left" shift="left"
                        isOpen={host.showConfiguration}>
                        <button
                          class="remove-host"
                          onClick={() => this.toggleHostConfiguration(host.id, true)}
                        >
                          <app-icon type="cog" scale={1.667} />
                        </button>
                        <div slot="popup-header">Host Configuration</div>
                        <smtp-host-configuration slot="popup-body"
                          value={host}
                          onUpdateHost={e => this.updateHost(e.detail)}
                        />
                        <div class="buttons" slot="popup-footer">
                          <app-popup position="right">
                            <button class="delete warning">
                              <app-icon type="delete" /> Delete
                            </button>
                            <button slot="popup-body"
                              class="confirm danger"
                              onClick={() => this.removeHost(host.id)}>
                              Confirm
                              </button>
                          </app-popup>
                        </div>
                      </app-popup>
                    }
                  </div>
                </li>
              ))}
            </ul>
            <div class="hosts-actions">
              <button class="add-host primary" onClick={() => {
                this.newHostId = newId();
                this.addHost({ id: this.newHostId });
              }}>
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
        onShutdownWorker={() => this.shutdownWorker()}
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
