import { Component, Prop, h } from '@stencil/core';
import { AppIcons } from './model';

@Component({
  tag: 'app-icon',
  styleUrl: 'component.css',
  shadow: true
})
export class AppIconComponent {
  @Prop() type: AppIcons;
  @Prop({ reflect: true }) rotate: number = 0;
  @Prop({ reflect: true }) flipHorizontal: boolean = false;
  @Prop({ reflect: true }) flipVertical: boolean = false;

  get(type: string) {
    switch (type) {
      default:
        return <text>{type}</text>;

      case 'arrow':
        return <path d="M10.5,3 15,7.5 10.5,12 10.5,9 0,9 0,6 10.5,6Z" />;

      case 'bowtie':
        return <path d="M 0,0 15,15 15,0 0,15Z" />;

      case 'circle':
        return <circle cx="7.5" cy="7.5" r="7.5" />;

      case 'close':
        return (
          <path d="M 1,3 3,1 7.5,5.5 12,1 14,3 9.5,7.5 14,12 12,14 7.5,9.5 3,14 1,12 5.5,7.5 Z" />
        );

      case 'expand':
        return (
          <g>
            <path d="M 7.5,0 2,6 13,6 Z" />
            <path d="M 7.5,15 2,9 13,9 Z" />
          </g>
        );

      case 'collapse':
        return (
          <g>
            <path d="M 7.5,6 2,0 13,0 Z" />
            <path d="M 7.5,9 2,15 13,15 Z" />
          </g>
        );

      case 'chevron':
        return <path d="M 0,4 15,4 7.5,12 Z" />;

      case 'chevron-dbl':
        return (
          <g>
            <path d="M 0,0 15,0 7.5,7.5Z" />,
            <path d="M 0,7.5 15,7.5 7.5,15Z" />
          </g>
        );

      case 'ellipsis':
        return (
          <g>
            <circle cx="1" cy="7.5" r="2" />
            <circle cx="7.5" cy="7.5" r="2" />
            <circle cx="14" cy="7.5" r="2" />
          </g>
        );

      case 'minus':
        return <path d="M 0,6 15,6 15,9 0,9Z" />;

      case 'paperclip':
        return (
          <path
            d="M3,12A3,3 1,0,0 6,15
              L9,15A3,3 1,0,0 12,12
              L12,3A3,3 1,0,0 9,0
              L7,0A2,2 1,0,0 5,2
              L5,11A1,1 1,0,0 6,12
              L9,12A1,1 1,0,0 10,11
              L10,4A.25,.25 1,0,0 9,4 L9,11 6,11
              L6,2A1,1 1,0,1 7,1
              L9,1A2,2 1,0,1 11,3
              L11,12A2,2 1,0,1 9,14
              L6,14A2,2 1,0,1 4,12
              L4,4A.25,.25 1,0,0 3,4Z"
          />
        );

      case 'plus':
        return (
          <path d="M 6,0 9,0 9,6 15,6 15,9 9,9 9,15 6,15 6,9 0,9 0,6 6,6Z" />
        );

      case 'rotate':
        return (
          <path
            d="M5.5,3.5 4,5 4,0 9,0 7.5,1.5
                A9.5,9.5 0 0,1 7.5,13.5
                M7.5,13.5 9,15 4,15 4,10 5.5,11.5
                A7,7 0 0,0 5.5,3.5"
          />
        );
      case 'square':
        return <path d="M1,1 14,1 14,14 1,14 Z" />;

      case 'star':
        return <path d="M7.5,0 12.5,15 0,5 15,5 2.5,15Z" />;

      case 'triangle':
        return <path d="M7.5,1 15,14 0,14 Z" />;
    }
  }

  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 15 15">
        <g
          transform={`rotate(${this.rotate},7.5,7.5) scale(${
            this.flipHorizontal ? '-1' : '1'
          },${this.flipVertical ? '-1' : '1'}) translate(${
            this.flipHorizontal ? '-15' : '0'
          },${this.flipVertical ? '-15' : '0'})`}
        >
          {this.get(this.type)}
        </g>
      </svg>
    );
  }
}
