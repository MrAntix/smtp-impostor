import { newE2EPage, E2EPage } from '@stencil/core/testing';
import { HubState, IHubSocket } from './model';

describe('impostor-hub', () => {
  it('renders', async () => {
    const page = await setup();

    const element = await page.find('impostor-hub');

    expect(element).toHaveClass('hydrated');
  });

  it('connect', async () => {
    const socket: IHubSocket = {
      send: () => {},
      close: () => {}
    };

    const page = await setup(socket);

    const element = await page.find('impostor-hub');
    const stateChanged = await element.spyOnEvent('stateChanged');

    await element.callMethod('connectAsync');
    socket.onopen({});

    await page.waitForChanges();

    const state = await element.getProperty('state');

    expect(state).toEqual(HubState.connected);
    expect(stateChanged).toHaveReceivedEventDetail(HubState.connected);
  });

  async function setup(socket?: IHubSocket): Promise<E2EPage> {
    const page = await newE2EPage();
    await page.setContent('<impostor-hub></impostor-hub>');

    if (socket) {
      page.evaluate(
        socket => {
          var element = document.querySelector('impostor-hub');
          element['socketProvider'] = _ => socket;
          window['fakeSocket'] = socket;
        },
        socket as any
      );

      socket.onopen = e => {
        page.evaluate(e => {
          window['fakeSocket'].onopen(e);
        }, e);
      };
    }

    return page;
  }
});
