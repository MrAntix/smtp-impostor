import { Component, h } from '@stencil/core';

@Component({
  tag: 'app-home',
  styleUrl: 'app-home.css',
  shadow: true
})
export class AppHome {

  render() {
    return (
      <div class='app-home page'>
        <h2>A fake SMTP server for developers</h2>
        <p>
          Catches emails sent via SMTP an puts them in a temp directory so you don't send people emails by accident
        </p>
      </div>
    );
  }
}
