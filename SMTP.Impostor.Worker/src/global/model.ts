export enum LogLevel {
  debug,
  info,
  warn,
  error
}

export interface ILogMethod {
  (...args: any[]): void;
}
export interface ILogMethodPovider {
  (level: LogLevel): ILogMethod;
}

export interface ILogger {
  debug: ILogMethod;
  info: ILogMethod;
  warn: ILogMethod;
  error: ILogMethod;
}

export function consoleLogMethodProvider(level: LogLevel) {
  return (args: any[]) => console[LogLevel[level]](...args);
}

export function newId(): string {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
    const r = (Math.random() * 16) | 0;
    const v = c === 'x' ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}
