export interface IHubSocket {
  onopen?: ((e: any) => any) | null;
  onclose?: ((e: { reason: string }) => any) | null;
  onerror?: ((e: any) => any) | null;
  onmessage?: ((e: { data: string }) => any) | null;
  readyState?: number;
  close(code?: number, reason?: string): void;
  send(data: string | ArrayBufferLike | Blob | ArrayBufferView): void;
}

export interface IHubSocketProvider {
  (url: string): IHubSocket;
}

export const hubSocketProvider: IHubSocketProvider = url => new WebSocket(url);

export enum HubStatus {
  disconnected,
  connecting,
  connected,
  error
}

export interface IHubMessage {
  type: string;
  model?: any;
}
