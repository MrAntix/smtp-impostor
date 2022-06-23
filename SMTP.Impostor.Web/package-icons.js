const { convertFile } = require('convert-svg-to-png');
const imagemin = require('imagemin');
const imageminPngquant = require('imagemin-pngquant');
const fs = require('fs');
const toIco = require('png-to-ico');

(async () => {
  await convertToPNG('icon', 24);
  await convertToPNG('icon', 48);
  await convertToPNG('icon', 96);
  await convertToPNG('icon', 256);
  await convertToPNG('icon', 512);
  await convertToPNG('icon', 1024);
  await convertToICO('icon', [24, 48, 96, 256]);
  await convertToPNG('a', 512);
  await convertToPNG('sender-icon', 24);
  await convertToPNG('sender-icon', 48);
  await convertToPNG('sender-icon', 96);
  await convertToPNG('sender-icon', 256);
  await convertToICO('sender-icon', [24, 48, 96, 256]);

  await imagemin(['./wwwroot/assets/*.png'], './wwwroot/assets/', {
    plugins: [
      imageminPngquant()
    ]
  });
})();

async function convertToPNG(name, width, height, output) {
  height = height || width;
  output = output || `./wwwroot/assets/${name}-${width}x${height}.png`;

  await convertFile(`./src/assets/${name}.svg`, {
    width,
    height,
    outputFilePath: output
  });

  console.log(`created png ${output}`)
}

async function convertToICO(name, sizes) {
  const files = sizes.map(size =>
    fs.readFileSync(`./wwwroot/assets/${name}-${size}x${size}.png`),
  );

  const output = `./wwwroot/assets/${name}.ico`;

  toIco(files).then(buf => {
    fs.writeFileSync(output, buf);
  });

  console.log(`created icon ${output}`)
}
