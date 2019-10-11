import { IStore, serializeDate, deserializeDate } from './model';
import { LocalStorageStore } from './service';

describe('LocalStorageStore', () => {
  const store: IStore = new LocalStorageStore();
  const KEY = 'KEY';

  [true, 0, 1, 'STRING', { prop: 'PROP' }].forEach(value => {
    it(`stores and retrieves ${JSON.stringify(value)}`, async () => {
      await store.putAsync(KEY, value);
      var storedValue = await store.getAsync(KEY);

      expect(storedValue).toEqual(value);
    });
  });

  it('stores and retrieves with parse/serialize', async () => {
    const value = new Date();

    await store.putAsync(KEY, value, serializeDate);
    var storedValue = await store.getAsync(KEY, deserializeDate);

    expect(storedValue).toEqual(value);
  });
});
