import { b as bootstrapLazy } from './p-3b0b096b.js';
import { p as patchBrowser, g as globalScripts } from './p-4cbdf50e.js';
import './p-df055c62.js';

patchBrowser().then(options => {
  globalScripts();
  return bootstrapLazy([["p-823f2846",[[1,"app-root",{"state":[32]}],[1,"smtp-host",{"value":[16],"showMessages":[516,"show-messages"],"showConfiguration":[516,"show-configuration"],"toggleState":[64],"searchMessages":[64]}],[1,"impostor-hub",{"socketProvider":[16],"url":[1],"status":[2],"connectAsync":[64],"sendAsync":[64],"disconnectAsync":[64]}],[1,"smtp-host-configuration",{"value":[16]}],[1,"app-input",{"placeholder":[513],"clearButton":[516,"clear-button"],"iconType":[513,"icon-type"],"value":[1]}],[1,"app-popup",{"isOpen":[1540,"is-open"],"modal":[4],"position":[1537],"shift":[1537],"toggle":[64]}],[1,"app-icon",{"type":[1],"rotate":[514],"flipHorizontal":[516,"flip-horizontal"],"flipVertical":[516,"flip-vertical"],"scale":[514]}]]]], options);
});
