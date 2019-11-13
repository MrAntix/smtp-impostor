import { Component, Prop, h } from "@stencil/core";
import { AppIcons } from "./model";

@Component({
  tag: "app-icon",
  styleUrl: "component.css",
  shadow: true
})
export class AppIconComponent {
  @Prop() type: AppIcons;
  @Prop({ reflect: true }) rotate: number = 0;
  @Prop({ reflect: true }) flipHorizontal: boolean = false;
  @Prop({ reflect: true }) flipVertical: boolean = false;
  @Prop({ reflect: true }) scale: number = 1;

  get(type: string) {
    switch (type) {
      default:
        return <text>{type}</text>;

      case "check":
        return <path d="M5,14 0,9 2,7 5,10 13,2 15,4Z" />;

      case "close":
        return (
          <path d="M 1,3 3,1 7.5,5.5 12,1 14,3 9.5,7.5 14,12 12,14 7.5,9.5 3,14 1,12 5.5,7.5 Z" />
        );

      case "cog":
        return [
          <path
            d="M14.991,7.875 A7.5,7.5 0,0,1 14.174,10.921
                L12.456,10.702 A5.9,5.9 0,0,0 13.393,7.205Z M10.921,14.174 A7.5,7.5 0,0,1 7.875,14.991
                L7.205,13.393 A5.9,5.9 0,0,0 10.702,12.456Z M3.43,13.8 A7.5,7.5 0,0,1 1.2,11.57
                L2.249,10.191 A5.9,5.9 0,0,0 4.809,12.751Z M0.009,7.125 A7.5,7.5 0,0,1 0.826,4.079
                L2.544,4.298 A5.9,5.9 0,0,0 1.607,7.795Z M4.079,0.826 A7.5,7.5 0,0,1 7.125,0.009
                L7.795,1.607 A5.9,5.9 0,0,0 4.298,2.544Z M11.57,1.2 A7.5,7.5 0,0,1 13.8,3.43
                L12.751,4.809 A5.9,5.9 0,0,0 10.191,2.249Z"
          ></path>,
          <path d="M7.5,1.5 A6,6 0,0,1 7.5,13.5 M7.5,1.5 A6,6 0,0,0 7.5,13.5 M7.5,10.5 A3,3 0,0,1 7.5,4.5 M7.5,10.5 A3,3 0,0,0 7.5,4.5"></path>
        ];

      case "delete":
        return (
          <path d="M 1,3 3,1 7.5,5.5 12,1 14,3 9.5,7.5 14,12 12,14 7.5,9.5 3,14 1,12 5.5,7.5 Z" />
        );

      case "paperclip":
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

      case "plus":
        return (
          <path d="M 6,0 9,0 9,6 15,6 15,9 9,9 9,15 6,15 6,9 0,9 0,6 6,6Z" />
        );

      case "search":
        return (
          <path
            d="M6,0 A1,1 0,0,0 6,12
                  M6,12 A1,1 0,0,0 6,0
                  M6,1.25 A1,1 0,0,1 6,10.75
                  M6,10.75 A1,1 0,0,1 6,1.25
                  M10,9 9,10 14,15 15,14Z"
          />
        );

      case "triangle":
        return <path d="M7.5,1 15,14 0,14 Z" />;
    }
  }

  render() {
    const scaleX = this.scale * (this.flipHorizontal ? -1 : 1);
    const scaleY = this.scale * (this.flipVertical ? -1 : 1);

    return (
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 15 15">
        <g
          transform={`
            translate(${7.5 - (15 / 2) * scaleX}, ${7.5 - (15 / 2) * scaleY})
            rotate(${this.rotate},${(15 / 2) * scaleX},${(15 / 2) * scaleY})
            scale(${scaleX},${scaleY})`}
        >
          {this.get(this.type)}
        </g>
      </svg>
    );
  }
}
