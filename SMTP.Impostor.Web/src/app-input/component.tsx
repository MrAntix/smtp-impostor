import { Component, Prop, h, Event, EventEmitter, Host } from '@stencil/core';
import { AppIcons } from '../app-icon';

@Component({
  tag: 'app-input',
  styleUrl: 'component.css',
  shadow: true
})
export class AppInputComponent {

  @Prop({ reflect: true }) placeholder: string;
  @Prop({ reflect: true }) clearButton: boolean = false;
  @Prop({ reflect: true }) iconType?: AppIcons = null;

  @Prop() value: string = '';

  render() {

    return <Host>
      <input value={this.value} placeholder={this.placeholder}
        onInput={(e: any) => this.inputType.emit(e.target.value)}
        onChange={(e: any) => this.inputChange.emit(e.target.value)}
        onKeyDown={e => {
          e.key === 'Escape' && this.inputClear.emit();
        }
        } />
      {(this.clearButton && !!this.value) &&
        <button class="clear"
          onClick={() => this.inputClear.emit()}>
          <app-icon type="close" />
        </button>
      }
      {this.iconType &&
        <app-icon class="icon" type={this.iconType} />
      }
    </Host>
  }

  @Event() inputType: EventEmitter<string>;
  @Event() inputChange: EventEmitter<string>;
  @Event() inputClear: EventEmitter<void>;
}

