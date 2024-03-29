import { newE2EPage, E2EPage } from '@stencil/core/testing';
import { HostStatus } from '../redux';

describe('smtp-host', () => {
  it('renders', async () => {
    const page = await setup();
    const element = await page.find('smtp-host');

    expect(element).toHaveClass('hydrated');
  });

  it('startHost', async () => {
    const page = await setup();
    const element = await page.find('smtp-host');
    const host = { state: HostStatus.Stopped };

    element.setProperty('value', host);
    await page.waitForChanges();

    const onStartHost = await element.spyOnEvent('startHost');

    const button = await page.find('smtp-host >>> .toggle-state');
    await button.click();

    expect(onStartHost).toHaveReceivedEventDetail(host);
  });

  it('stopHost', async () => {
    const page = await setup();
    const element = await page.find('smtp-host');
    const host = { state: HostStatus.Started };

    element.setProperty('value', host);
    await page.waitForChanges();

    const onStopHost = await element.spyOnEvent('stopHost');

    const button = await page.find('smtp-host >>> .toggle-state');
    await button.click();

    expect(onStopHost).toHaveReceivedEventDetail(host);
  });

  async function setup(): Promise<E2EPage> {
    const page = await newE2EPage();
    await page.setContent('<smtp-host></smtp-host>');

    return page;
  }
});
