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
