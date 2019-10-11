export interface IStore {
  getAsync<TData>(
    key: string,
    deserialize?: (v: string) => TData
  ): Promise<TData>;
  putAsync<TData>(
    key: string,
    data: TData,
    serialize?: (d: TData) => string
  ): Promise<void>;
  deleteAsync(key: string): Promise<void>;
}

export function serializeDate(value: Date): string {
  return value.toISOString();
}

export function deserializeDate(value: string): Date {
  return new Date(Date.parse(value));
}
