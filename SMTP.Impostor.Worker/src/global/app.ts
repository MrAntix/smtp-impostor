import { LogLevel, consoleLogMethodProvider, ILogger } from './model';

export default async () => {
  globalThis.logScope = { default: LogLevel.debug };
  globalThis.loggerMethodProvider = consoleLogMethodProvider;

  globalThis.getLogger = (scope: string): ILogger => {
    const getMethod = (methodLevel: LogLevel) => {
      const method = globalThis.loggerMethodProvider(methodLevel);

      return (...args: any[]) => {
        const scopeLevel = Reflect.has(globalThis.logScope, scope)
          ? globalThis.logScope[scope]
          : globalThis.logScope.default;
        if (methodLevel >= scopeLevel)
          method([`[${LogLevel[methodLevel]}] ${scope}`, ...args]);
      };
    };

    return {
      debug: getMethod(LogLevel.debug),
      info: getMethod(LogLevel.info),
      warn: getMethod(LogLevel.warn),
      error: getMethod(LogLevel.error)
    };
  };
};
