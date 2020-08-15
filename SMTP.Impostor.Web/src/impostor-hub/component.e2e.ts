import { newE2EPage, E2EPage } from '@stencil/core/testing';
import { HubStatus, IHubSocket } from './model';

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
    const statusChanged = await element.spyOnEvent('statusChanged');

    await element.callMethod('connectAsync');
    socket.onopen({});

    await page.waitForChanges();

    const status = await element.getProperty('status');

    expect(status).toEqual(HubStatus.connected);
    expect(statusChanged).toHaveReceivedEventDetail(HubStatus.connected);
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
