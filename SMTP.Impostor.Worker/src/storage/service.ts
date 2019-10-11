import { IStore } from './model';

export class LocalStorageStore implements IStore {
  getAsync<TData>(
    key: string,
    deserialize: (v: string) => TData = JSON.parse
  ): Promise<TData> {
    var value = window.localStorage.getItem(key);
    return Promise.resolve(deserialize(value));
  }

  putAsync<TData>(
    key: string,
    data: TData,
    serialize: (d: TData) => string = JSON.stringify
  ): Promise<void> {
    window.localStorage.setItem(key, serialize(data));
    return Promise.resolve();
  }

  deleteAsync(key: string): Promise<void> {
    window.localStorage.removeItem(key);
    return Promise.resolve();
  }
}
