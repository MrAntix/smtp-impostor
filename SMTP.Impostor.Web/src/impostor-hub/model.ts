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
  working = -1,
  disconnected,
  connecting,
  connected,
  error
}

export function hubStatusDisplay(
  status: HubStatus
) {
  switch (status) {
    default: return null;
    case HubStatus.working: return 'Working, please wait...';
    case HubStatus.disconnected: return 'Disconnected';
    case HubStatus.connecting: return 'Connecting...';
    case HubStatus.connected: return 'Connected';
    case HubStatus.error: return 'Error';
  }
}

export interface IHubMessage {
  type: string;
  model?: any;
}

