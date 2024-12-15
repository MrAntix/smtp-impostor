export { L as LogLevel, c as consoleLogMethodProvider, n as newId } from './p-adcbd502.js';
export { D as DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA, p as HostStatus, H as HubStatus, T as Types, b as addHost, c as configureStore, j as deleteHostMessage, d as dispatch, v as getInitialState, q as hostIsRunning, h as hubSocketProvider, a as hubStatusDisplay, i as initWorkerState, k as loadHostMessage, l as loadWorkerState, o as openHost, e as removeHost, r as rootReducer, g as searchHostMessages, n as shutdownWorker, s as startHost, m as startupWorker, f as stopHost, t as toggleHostConfiguration, u as updateHost } from './p-794aa3e2.js';

function serializeDate(value) {
  return value.toISOString();
}
function deserializeDate(value) {
  return new Date(Date.parse(value));
}

class LocalStorageStore {
  getAsync(key, deserialize = JSON.parse) {
    var value = window.localStorage.getItem(key);
    return Promise.resolve(deserialize(value));
  }
  putAsync(key, data, serialize = JSON.stringify) {
    window.localStorage.setItem(key, serialize(data));
    return Promise.resolve();
  }
  deleteAsync(key) {
    window.localStorage.removeItem(key);
    return Promise.resolve();
  }
}

export { LocalStorageStore, deserializeDate, serializeDate };

//# sourceMappingURL=index.esm.js.map