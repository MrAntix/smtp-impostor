const { convertFile } = require('convert-svg-to-png');
const imagemin = require('imagemin');
const imageminPngquant = require('imagemin-pngquant');
const fs = require('fs');
const toIco = require('to-ico');

(async () => {
  await convertToPNG('icon', 24);
  await convertToPNG('icon', 48);
  await convertToPNG('icon', 96);
  await convertToPNG('icon', 256);
  await convertToPNG('icon', 512);
  await convertToPNG('icon', 1024);
  await convertToPNG('icon', 24, 24, '../SMTP.Impostor.Packager/Images/StoreLogo.png');
  await convertToPNG('icon', 48, 48, '../SMTP.Impostor.Packager/Images/LockScreenLogo.scale-200.png');
  await convertToPNG('icon', 50, 50, '../SMTP.Impostor.Packager/Images/Square44x44Logo.targetsize-24_altform-unplated.png');
  await convertToPNG('icon', 88, 88, '../SMTP.Impostor.Packager/Images/Square44x44Logo.scale-200.png');
  await convertToPNG('icon', 300, 300, '../SMTP.Impostor.Packager/Images/Square150x150Logo.scale-200.png');
  await convertToPNG('icon', 620, 300, '../SMTP.Impostor.Packager/Images/Wide310x150Logo.scale-200.png');
  await convertToPNG('icon', 1240, 600, '../SMTP.Impostor.Packager/Images/SplashScreen.scale-200.png');
  await convertToICO('icon', [24, 48, 96, 256]);
  await convertToPNG('a', 512);

  await imagemin(['./www/assets/*.png'], './www/assets/', {
    plugins: [
      imageminPngquant()
    ]
  });
})();

async function convertToPNG(name, width, height, output) {
  height = height || width;
  output = output || `./www/assets/${name}-${width}x${height}.png`;

  await convertFile(`./src/assets/${name}.svg`, {
    width,
    height,
    outputFilePath: output
  });

  console.log(`created png ${output}`)
}

async function convertToICO(name, sizes) {
  const files = sizes.map(size =>
    fs.readFileSync(`./www/assets/${name}-${size}x${size}.png`),
  );

  const output = `./www/assets/${name}.ico`;

  toIco(files).then(buf => {
    fs.writeFileSync(output, buf);
  });

  console.log(`created icon ${output}`)
}
