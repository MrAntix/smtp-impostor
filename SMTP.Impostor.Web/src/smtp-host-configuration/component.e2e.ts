import { newE2EPage, E2EPage } from '@stencil/core/testing';

describe('smtp-host-configuration', () => {
  it('renders', async () => {
    const page = await setup();
    const element = await page.find('smtp-host-configuration');

    expect(element).toHaveClass('hydrated');
  });

  ['ip', 'port', 'name'].forEach(inputName => {
    it(`configuration ${inputName}`, async () => {
      const page = await setup();
      const element = await page.find('smtp-host-configuration');

      element.setProperty('value', { });
      await page.waitForChanges();

      const onUpdateHost = await element.spyOnEvent('updateHost');

      const input = await page.find(`smtp-host-configuration >>> input[name="${inputName}"]`);
      await input.type(inputName);

      await page.keyboard.press('Tab');

      expect(onUpdateHost).toHaveReceivedEventDetail({ [inputName]: inputName });
    });
  })

  async function setup(): Promise<E2EPage> {
    const page = await newE2EPage();
    await page.setContent('<smtp-host-configuration></smtp-host-configuration>');

    return page;
  }
});
