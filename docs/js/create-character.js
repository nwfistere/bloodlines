// import { downloadUrl, FieldsToZipFile } from "./zipper";

const equipmentJson = {
  "equipment": [ "VOID", "MAGIC_MISSILE", "HOLY_MISSILE", "WHIP", "VAMPIRICA", "AXE", "SCYTHE", "KNIFE", "THOUSAND", "HOLYWATER", "BORA", "DIAMOND", "FIREBALL", "HELLFIRE", "HOLYBOOK", "VESPERS", "CROSS", "HEAVENSWORD", "GARLIC", "VORTEX", "LAUREL", "THORNS", "LIGHTNING", "LOOP", "PENTAGRAM", "ROSARY", "SIRE", "SILF", "SILF2", "SILF3", "BONE", "LANCET", "SONG", "MANNAGGIA", "CHERRY", "CART", "CART2", "GATTI", "STIGRANGATTI", "GATTI_SCRATCH", "GATTI_SCUFFLE", "FLOWER", "GUNS", "GUNS2", "GUNS3", "TRAPANO", "TRAPANO2", "SARABANDE", "FIREEXPLOSION", "NDUJA", "POWER", "AREA", "SPEED", "COOLDOWN", "DURATION", "AMOUNT", "MAXHEALTH", "ARMOR", "MOVESPEED", "MAGNET", "GROWTH", "LUCK", "GREED", "REVIVAL", "SHIELD", "REGEN", "CURSE", "SILVER", "GOLD", "ROCHER", "ROCHEREXP", "LEFT", "RIGHT", "CORRIDOR", "SHROUD", "COLDEXPLOSION", "WINDOW", "PANDORA", "VENTO", "VENTO2", "VENTO2_EXPLO", "VENTO2_EXTRA", "ROBBA", "WINDOW2", "SILF_COUNTER", "SILF2_COUNTER", "RAYEXPLOSION", "CANDYBOX", "GUNS_COUNTER", "GUNS2_COUNTER", "JUBILEE", "VICTORY", "SOLES", "GATTI_COUNTER", "NDUJA_COUNTER", "TRIASSO1", "TRIASSO2", "TRIASSO3", "BLOODLINE", "CANDYBOX2", "BUBBLES", "ASTRONOMIA", "BLOOD_GARLIC", "BLOOD_SONG", "BLOOD_PENTAGRAM", "BLOOD_LANCET", "BLOOD_LAUREL", "JUBILEE_RAYS", "MISSPELL", "MISSPELL2", "SILVERWIND", "SILVERWIND2", "FOURSEASONS", "FOURSEASONS2", "SUMMONNIGHT", "SUMMONNIGHT2", "MIRAGEROBE", "MIRAGEROBE2", "BUBBLES2", "NIGHTSWORD", "NIGHTSWORD2", "BOCCE", "BOCCE_COUNTER", "SPELL_STRING", "SPELL_STREAM", "SPELL_STRIKE", "SPELL_STROM", "BONE2", "BONE_ARM", "TETRAFORCE", "SHADOWSERVANT", "SHADOWSERVANT2", "PRISMATICMISS", "PRISMATICMISS2", "FLASHARROW", "FLASHARROW2", "SEC_MILLIONAIRE", "SWORD", "SWORD2", "PARTY", "SWORD_FINISHER", "LEGIONNAIRE", "PHASER", "SHADOWSERVANT_COUNTER", "PARTY_COUNTER", "ACADEMYBADGE", "INSATIABLE", "CHERRY2", "CHERRY_STAR_EXPLO", "CHERRY_STAR" ],
  "passives": ["POWER", "REGEN", "MAXHEALTH", "ARMOR", "AREA", "SPEED", "COOLDOWN", "DURATION", "AMOUNT", "MOVESPEED", "MAGNET", "LUCK", "GROWTH", "GREED", "CURSE", "REVIVAL", "REROLL", "SKIP", "BANISH", "CHARM"],
  "weapons": ["MAGIC_MISSILE","HOLY_MISSILE","WHIP","VAMPIRICA","AXE","SCYTHE","KNIFE","THOUSAND","HOLYWATER","BORA","DIAMOND","FIREBALL","HELLFIRE","HOLYBOOK","VESPERS","CROSS","HEAVENSWORD","GARLIC","VORTEX","LAUREL","THORNS","LIGHTNING","LOOP","PENTAGRAM","ROSARY","SIRE","SILF","SILF2","SILF3","BONE","LANCET","SONG","MANNAGGIA","CHERRY","CART","CART2","GATTI","STIGRANGATTI","GATTI_SCRATCH","GATTI_SCUFFLE","FLOWER","GUNS","GUNS2","GUNS3","TRAPANO","TRAPANO2","SARABANDE","FIREEXPLOSION","NDUJA","SHIELD","SILVER","GOLD","ROCHER","ROCHEREXP","LEFT","RIGHT","CORRIDOR","SHROUD","COLDEXPLOSION","WINDOW","VENTO","VENTO2","VENTO2_EXPLO","VENTO2_EXTRA","ROBBA","WINDOW2","SILF_COUNTER","SILF2_COUNTER","RAYEXPLOSION","CANDYBOX","GUNS_COUNTER","GUNS2_COUNTER","JUBILEE","VICTORY","SOLES","GATTI_COUNTER","NDUJA_COUNTER","TRIASSO1","TRIASSO2","TRIASSO3","BLOODLINE","CANDYBOX2","BUBBLES","ASTRONOMIA","BLOOD_GARLIC","BLOOD_SONG","BLOOD_PENTAGRAM","BLOOD_LANCET","BLOOD_LAUREL","JUBILEE_RAYS","MISSPELL","MISSPELL2","SILVERWIND","SILVERWIND2","FOURSEASONS","FOURSEASONS2","SUMMONNIGHT","SUMMONNIGHT2","MIRAGEROBE","MIRAGEROBE2","BUBBLES2","NIGHTSWORD","NIGHTSWORD2","BOCCE","BOCCE_COUNTER","SPELL_STRING","SPELL_STREAM","SPELL_STRIKE","SPELL_STROM","BONE2","BONE_ARM","TETRAFORCE","SHADOWSERVANT","SHADOWSERVANT2","PRISMATICMISS","PRISMATICMISS2","FLASHARROW","FLASHARROW2","SEC_MILLIONAIRE","SWORD","SWORD2","PARTY","SWORD_FINISHER","LEGIONNAIRE","PHASER","SHADOWSERVANT_COUNTER","PARTY_COUNTER","ACADEMYBADGE","INSATIABLE","CHERRY2","CHERRY_STAR_EXPLO","CHERRY_STAR"],
  "baseWeapons": ["AXE", "CROSS", "HOLYBOOK", "FIREBALL", "GARLIC", "HOLYWATER", "DIAMOND", "LIGHTNING", "PENTAGRAM", "SILF", "SILF2", "GUNS", "GUNS2", "GATTI", "SONG", "TRAPANO", "LANCET", "LAUREL", "VENTO", "BONE", "CHERRY", "CART2", "FLOWER", "LAROBBA", "JUBILEE", "TRIASSO1", "VICTORY", "MISSPELL", "SILVERWIND", "FOURSEASONS", "SUMMONNIGHT", "MIRAGEROBE", "BUBBLES", "NIGHTSWORD", "BOCCE", "SPELL_STRING", "SPELL_STREAM", "SPELL_STRIKE", "SWORD", "FLASHARROW", "PRISMATICMISS", "SHADOWSERVANT", "PARTY"],
  "counterpartWeapons": ["SILF_COUNTER", "SILF2_COUNTER", "SILF3_COUNTER", "GUNS_COUNTER", "GUNS2_COUNTER", "GUNS3_COUNTER", "GATTI_COUNTER", "NDUJA_COUNTER", /*"BOCCE_COUNTER",*/ "SHADOWSERVANT_COUNTER", "PARTY_COUNTER"]
};

const characterJson = {
  "version": "0.1",
  "character": [
    {
      "level": 1,
      "startingWeapon": "",
      "prefix": "",
      "charName": "",
      "surname": "",
      "textureName": "characters",
      "spriteName": "",
      "skins": [],
      "currentSkinIndex": 0,
      "walkingFrames": 1,
      "description": "",
      "isBought": true,
      "price": 0,
      "completedStages": [],
      "survivedMinutes": 0,
      "enemiesKilled": 0,
      "stageData": [],
      "showcase": [],
      "hiddenWeapons": [],
      "bodyOffset": {
        x: 0,
        y: 0
      },
      "statModifiers": []
    },
  ]
};

const statModifierJson = {
  "level": 0,
  "maxHp": 0,
  "armor": 0,
  "regen": 0,
  "moveSpeed": 0,
  "power": 0,
  "cooldown": 0,
  "area": 0,
  "speed": 0,
  "duration": 0,
  "amount": 0,
  "luck": 0,
  "growth": 0,
  "greed": 0,
  "curse": 0,
  "magnet": 0,
  "revivals": 0,
  "rerolls": 0,
  "skips": 0,
  "banish": 0,
  "shields": 0,
  "charm": 0
};

const skinJson = {
    "id": 0,
    "name": "Default",
    "textureName": "characters",
    "spriteName": "",
    "walkingFrames": 1,
    "unlocked": true,
    "frames": []
};

const spritePortraitString = "<i class=\"fas fa-file-image\"></i> Sprite Portrait";
const spriteFrameString = "<i class=\"fas fa-file-image\"></i> Frames";

let numOfSkins = 0;
let numOfModifiers = 0;

const clone = (obj) => { return JSON.parse(JSON.stringify(obj)); }

const createJsonOutput = (form) => {
  console.debug(`Running createJsonOutput on form: ${form.id}`);
  if (!form) {
    return;
  }

  const currentJson = document.getElementById("outputJsonTextArea");
  let json = {};

  if (currentJson == null || currentJson.textContent.length == 0) {
    json = clone(characterJson);
  } else {
    json = JSON.parse(currentJson.textContent);
  }

  const char = json.character[0];
  for (let field of form.elements) {
    if (!field.id || field.disabled || field.type === "reset" || field.type === "submit" || field.type === "button") {
      continue;
    }

    let fieldId = field.id;

    if (field.id.includes(".")) {
      const split = field.id.split(".");
      if (split.length > 2 || !Object.hasOwn(char, split[0]) || !Object.hasOwn(char[split[0]], split[1])) {
        console.error("Invalid input id: " + field.id);
      }
      char[split[0]][split[1]] = getFieldValue(field);
      continue;
    } else if (field.id.startsWith("mod-")) {
      // Modifier objects
      const modField = field.id.substring(field.id.indexOf("-") + 1, field.id.lastIndexOf("-"));

      const num = parseInt(field.id.substring(field.id.lastIndexOf("-") + 1, field.id.length));

      console.debug("modifier num: " + num);

      while (char.statModifiers[num] == null) {
        char.statModifiers.push({});
      }

      const modObject = char.statModifiers[num];
      const fieldValue = getFieldValue(field);

      if (fieldValue > 0) {
        modObject[modField] = fieldValue;
      }

      continue;
    } else if (field.id.startsWith("skin-")) {
      let skinFieldId = field.id;
      if (skinFieldId.endsWith("-input")) {
        skinFieldId = skinFieldId.substring(0, skinFieldId.indexOf("-input"));
      }
      
      skinField = skinFieldId.substring(skinFieldId.indexOf("-") + 1, skinFieldId.lastIndexOf("-"));
      const num = parseInt(skinFieldId.substring(skinFieldId.lastIndexOf("-") + 1, skinFieldId.length));

      if (isNaN(num)) {
        console.debug(`Invalid field: ${skinFieldId} ${num} ${skinField}`);
        return;
      }

      console.debug("skin num: " + num);

      while (char.skins[num] == null) {
        char.skins.push({});
      }

      const skinObject = char.skins[num];
      const fieldValue = getFieldValue(field);

      skinObject[skinField] = fieldValue;
      continue;
    }

    if (!Object.hasOwn(char, fieldId)) {
      console.error("Json object doesn't have property: " + field.id);
      continue;
    }

    char[fieldId] = getFieldValue(field);
  }

  setJsonOutputValue(json);
}

const getFieldValue = (field) => {
  switch (field.type) {
    case "text":
    case "select-one":
      return field.value;
    case "number":
      return parseFloat(field.value) || 0;
    case "checkbox":
      return field.checked;
    case "file":
      const retArray = [];
      if (field.files) {
        for (const file of field.files) {
          retArray.push(file.name);
        }
      }
      return retArray;
      // return field.value.split(/(\\|\/)/g).pop();
    case "select-multiple":
      if (showCaseChoices._baseId.endsWith(field.id)) {
        return showCaseChoices.getValue(true);
      }
    default:
      console.error("Unknown input type: " + field.type);
  }

  return null;
}

const setupStartingWeapon = () => {
  var startingWeapon = document.getElementById("startingWeapon");

  const sortedWeapons = equipmentJson.weapons.sort();

  const defaultOption = document.createElement("option");
  defaultOption.text = "Choose a Starting Weapon";
  defaultOption.value = "";
  defaultOption.hidden = true;
  defaultOption.selected = true;
  defaultOption.disabled = true;
  startingWeapon.add(defaultOption);

  for (const weapon of sortedWeapons) {
    const option = document.createElement("option");
    option.text = weapon;
    option.value = weapon;
    startingWeapon.add(option);
  }
};

const setJsonOutputValue = (json) => {
  const output = document.getElementById("outputJsonTextArea");
  // output.add
  output.textContent = JSON.stringify(json, null, 2);
  output.style.height = 0;
  output.style.height = (output.scrollHeight) + "px";
};

document.addEventListener("DOMContentLoaded", () => {
  setJsonOutputValue(clone(characterJson));
});

const createColumnDiv = () => {
  const div = document.createElement("div");
  div.setAttribute("class", "col-6 col-12-small");
  return div;
};

const createRowDiv = (classes) => {
  const div = document.createElement("div");
  div.setAttribute("class", "row");
  if (classes) {
    div.classList.add(...classes);
  }
  return div;
};

const createInput = (type, name, id, placeholder) => {
  const input = document.createElement("input");
  input.setAttribute("type", type);
  input.setAttribute("name", name);
  input.setAttribute("id", id);
  if (type !== "checkbox") {
    input.setAttribute("placeholder", placeholder);
  }
  return input;
}

const createLabel = (innerHTML) => {
  const label = document.createElement("label");
  label.innerHTML = innerHTML;
  return label;
}

const resetFileInput = (element) => {
  if (!element) {
    console.error("resetFileInput - Invalid element");
    return;
  }

  const resetAndSetString = (element, newStr) => {
    element.value = '';
    const label = document.getElementById(element.id + "-label");
    if (label) {
      label.innerHTML = newStr;
      label.classList.remove("file-label--active");
    }
  }

  if (element.id.startsWith("skin-spriteName")) {
    resetAndSetString(element, spritePortraitString);
  }
}

const setOnImageChange = (element) => {
  element.addEventListener("change", (ev) => {
    const elem = ev.target;
    const [file] = elem.files;
    if (file) {
      const label = document.getElementById(elem.id + "-label");
      if (label) {
        label.innerHTML = "<img id=\"blah\" src=\"" + URL.createObjectURL(file) + "\" alt=\"" + file.name + "\" /><input type=\"button\" class=\"image-remove-button\"value=\"remove\" onclick=\"resetFileInput(document.getElementById('" + elem.id + "'));\" />";
        label.classList.add("file-label--active");
      }
    }
  });
};

const createSkinElement = (labelName, inputId, inputType, required = false, readOnly = false, hidden = false, value = undefined) => {
  let label = null;
  if (inputType === "file" || inputType === "checkbox") {
    label = createLabel(labelName);
    label.setAttribute("for", inputId);
    label.setAttribute("class", inputType + "-label");
    label.id = inputId + "-label";
  }

  const input = createInput(inputType, inputId, inputId, labelName);
  input.required = required;
  input.readOnly = readOnly;
  if (inputType == "file") {
    input.accept="image/png";
    setOnImageChange(input);
  }

  if (value !== undefined) {
    input.value = value;
  }
  if (hidden) {
    input.className += " hide";
  }
  const div = createColumnDiv();
  if (label != null && inputType !== "checkbox") {
    div.appendChild(label);
  }
  div.appendChild(input);
  if (label != null && inputType === "checkbox") {
    div.appendChild(label);
  }
  return div;
}

const createDiv = ({id, classList }) => {
  const div = document.createElement("div");
  if (id) {
    div.id = id;
  }
  if (classList) {
    div.classList.add(...classList);
  }
  return div;
};

const createFramesDiv = (num) => {
  const row = createDiv({ classList: ["box", "align-center"]});

  const text = document.createElement("p");
  text.innerHTML = "Drag & drop individual walking frames here, they will be sorting alphabetically.";

  const inputContainer = document.createElement("div");
  inputContainer.id = `skin-frames-${num}`;
  inputContainer.classList.add(`skin-frames-${num}`);

  const input = document.createElement("input");
  input.type = "file";
  input.id = `skin-frames-${num}-input`;
  input.multiple = true;
  input.accept = "image/png";
  input.addEventListener("change", (ev) => {
    filesManager(ev.target.parentNode.id, ev.target.files);
    return;
  });

  const inputLabel = document.createElement("label");
  inputLabel.classList.add("button");
  inputLabel.setAttribute("for", input.id);
  inputLabel.innerHTML = "Or click to upload from your computer.";

  inputContainer.appendChild(input);
  inputContainer.appendChild(inputLabel);

  const removeContainer = createDiv({id: `skin-frames-${num}-rm-container`, classList: ["row", "aln-center"] });
  const gallery = createDiv({id: `skin-frames-${num}-gallery`, classList: ["gallery", "row", "aln-center"] });

  handleFrameDropbox(inputContainer);

  row.appendChild(text);
  row.appendChild(inputContainer);
  row.appendChild(removeContainer);
  row.appendChild(gallery);

  return row;
}

document.getElementById("addSkinForm").addEventListener("click", () => {
  const div = document.createElement("div");
  div.setAttribute("class", "box");
  div.setAttribute("id", "skin-form-" + numOfSkins);

  // div.appendChild(createLabel("Skin " + (numOfSkins + 1)));
  const header = document.createElement("h4");
  header.innerHTML = "Skin " + (numOfSkins + 1);
  div.appendChild(header);
  // div.appendChild(createSkinElement("Id", "skin-id-" + numOfSkins, "number", false, true, true));
  div.appendChild(createSkinElement("Skin name", "skin-name-" + numOfSkins, "text"));
  // div.appendChild(createSkinElement("Texture Name", "skin-textureName-" + numOfSkins, "text", false, true, true));
  div.appendChild(createSkinElement(spritePortraitString, "skin-spriteName-" + numOfSkins, "file"));
  // div.appendChild(createSkinElement("Walking Frames", "skin-walkingFrames-" + numOfSkins, "number", false, true, true));
  div.appendChild(createSkinElement("Unlocked", "skin-unlocked-" + numOfSkins, "checkbox"));
  div.appendChild(createFramesDiv(numOfSkins));

  document.getElementById("skinContainer").appendChild(div);
  // document.getElementById("skin-id-" + numOfSkins).value = numOfSkins;
  // document.getElementById("skin-textureName-" + numOfSkins).value = "characters";
  // document.getElementById("skin-walkingFrames-" + numOfSkins).value = 4;
  document.getElementById("skin-unlocked-" + numOfSkins).checked = true;
  numOfSkins += 1;
  removeHide("removeSkinForm");
});

document.getElementById("removeSkinForm").addEventListener("click", () => {
  if (numOfSkins > 0) {
    const element = document.getElementById("skin-form-" + (numOfSkins - 1));
    element.parentNode.removeChild(element);
    numOfSkins -= 1;
    if (numOfSkins === 0) {
      hide("removeSkinForm");
    }
    return false;
  }
  return true;
});


const createModifierElement = (labelName, inputId, required = false, step=0.1) => {
  const label = createLabel(capitalizeFirstLetter(labelName));
  label.setAttribute("for", inputId);
  const input = createInput("number", inputId, inputId);
  input.required = required;
  input.step = step;
  const div = createColumnDiv();
  div.appendChild(label);
  div.appendChild(input);
  return div;
}


const capitalizeFirstLetter = (string) => {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
 

const createModiferSection = (i) => {
  const div = document.createElement("div");
  div.setAttribute("class", "modifier-form-instance");
  div.setAttribute("id", "modifier-form-" + i);

  div.appendChild(createLabel("Stat Modifier " + (i + 1)));

  const ShiftByOne = ["level", "revivals", "skips", "banish", "rerolls"]

  for (const [key] of Object.entries(statModifierJson)) {
    div.appendChild(createModifierElement(key, "mod-" + key + "-" + i, key === "level", ShiftByOne.includes(key) ? 1 : 0.1));
  }

  return div;
}

document.getElementById("addModifierForm").addEventListener("click", () => {
  let i = numOfModifiers;
  const div = createModiferSection(i);
  document.getElementById("modifierContainer").appendChild(div);
  if (i == 0) {
    document.getElementById("mod-level-0").value = 1;
    document.getElementById("mod-level-0").readOnly = true;
  }
  numOfModifiers += 1;
  removeHide("removeModifierForm");
});

document.getElementById("removeModifierForm").addEventListener("click", () => {
  document.getElementById("spriteName").files
  if (numOfModifiers > 0) {
    const element = document.getElementById("modifier-form-" + (numOfModifiers - 1));
    element.parentNode.removeChild(element);
    numOfModifiers -= 1;
    if (numOfModifiers === 0) {
      hide("removeModifierForm");
    }
    return false;
  }
  return true;
});

let showCaseChoices = null;

const createChoices = () => {
  const defaults = () => {
    const choices = [];
    for (const item of equipmentJson.equipment) {
      choices.push({
        value: item,
        label: item
      });
    }
    return choices;
  }
  

  const element = document.querySelector("#showcase");
  showCaseChoices = new Choices(element, {
    removeItemButton: true,
    choices: defaults(),
    placeholder: true,
    placeholderValue: "Showcase items   ",
    itemSelectText: "",
    allowHTML: true,
    classNames: {
      button: "choices__button ignore-style",
      item: "choices__item" // button",
    },
    callbackOnCreateTemplates: (template) => {
      return {
        item: ({classNames}, data) => {
          const div = Choices.defaults.templates.item.call(this, showCaseChoices.config, data, showCaseChoices.config.removeItemButton);
          div.className += " button";
          if(div.hasChildNodes()) {
            for (const node of div.childNodes) {
              if (node.type === "button") {
                node.innerHTML = "<i class=\"fas fa-times\" style=\"color:#f56a6a\"></i>";
                break;
              }
            }
          }
          return div;
        }
      }
    }
  });

  const form = document.querySelector("#basic-form");
  element.addEventListener("addItem", (e) => {
    showCaseChoices.setChoices([{
      value: e.detail.value,
      label: e.detail.label,
    }], "value", "label", false);
    createJsonOutput(form);
  });

  element.addEventListener("removeItem", (e) => {
    showCaseChoices.setChoices(defaults(), "value", "label", true);
    createJsonOutput(form);
  });
};

const setupDownload = () => {
  const button = document.getElementById("downloadButton");
  const zipButton = document.getElementById("downloadZipButton");

  button.addEventListener("click", () => {
    const data = [document.getElementById("outputJsonTextArea").textContent];
    const file = new File(data, "character.json", {
      type: "text/json",
    });

    // This is the url to download the image
    const blobURL = URL.createObjectURL(file);

    // Force the download
    const anchor = document.createElement("a");
    const clickEvent = new MouseEvent("click");
    anchor.href = blobURL;
    anchor.download = file.name;
    anchor.dispatchEvent(clickEvent);
  });

  zipButton.addEventListener("click", async () => {
    const obj = new FieldsToZipFile("character_pack", "outputJsonTextArea", ["spriteName"], ["skin-spriteName-", "skin-frames-", "skin-spriteName-"]);
    obj.zip();
  });


};

const removeHide = (elementId) => {
  const element = document.getElementById(elementId);
  let className = element.className;
  className = className.replace(/\b(hide)\b/g, "");
  element.className = className;
};

const hide = (elementId) => {
  const element = document.getElementById(elementId);
  element.className += " hide";
}


const handleFrameDropbox = (container) => {

	// modify all of the event types needed for the script so that the browser
	// doesn't open the image in the browser tab (default behavior)
	['dragenter', 'dragover', 'dragleave', 'drop'].forEach(evt => {
		container.addEventListener(evt, prevDefault, false);
	});
	function prevDefault (e) {
		e.preventDefault();
		e.stopPropagation();
	}

	// remove and add the hover class, depending on whether something is being
	// actively dragged over the box area
	['dragenter', 'dragover'].forEach(evt => {
		container.addEventListener(evt, hover, false);
	});
	['dragleave', 'drop'].forEach(evt => {
		container.addEventListener(evt, unhover, false);
	});
	function hover(e) {
		container.classList.add('hover');
	}
	function unhover(e) {
		container.classList.remove('hover');
	}

	// the DataTransfer object holds the data being dragged. it's accessible
	// from the dataTransfer property of drag events. the files property has
	// a list of all the files being dragged. put it into the filesManager function
	container.addEventListener('drop', mngDrop, false);
	function mngDrop(e) {
		let dataTrans = e.dataTransfer;
		let files = dataTrans.files;
		filesManager(container.id, files);
	}
}

// use FormData browser API to create a set of key/value pairs representing
// form fields and their values, to send using XMLHttpRequest.send() method.
// Uses the same format a form would use with multipart/form-data encoding
function upFile(file) {
  //only allow images to be dropped
  let imageType = /image.*/;
  if (file.type.match(imageType)) {
    let url = 'HTTP/HTTPS URL TO SEND THE DATA TO';
    // create a FormData object
    let formData = new FormData();
    // add a new value to an existing key inside a FormData object or add the
    // key if it doesn't exist. the filesManager function will loop through
    // each file and send it here to be added
    formData.append('file', file);

    // standard file upload fetch setup
    fetch(url, {
      method: 'put',
      body: formData
    })
    .then(response => response.json())
    .then(result => { console.log('Success:', result); })
    .catch(error => { console.error('Error:', error); });
  } else {
    console.error("Only images are allowed!", file);
  }
}


// use the FileReader API to get the image data, create an img element, and add
// it to the gallery div. The API is asynchronous so onloadend is used to get the
// result of the API function
function previewFile(idPrefix, file) {
  // only allow images to be dropped
  let imageType = /image.*/;
  if (file.type.match(imageType)) {
    let fReader = new FileReader();
    let gallery = document.getElementById(`${idPrefix}-gallery`);
    // reads the contents of the specified Blob. the result attribute of this
    // with hold a data: URL representing the file's data
    fReader.readAsDataURL(file);
    // handler for the loadend event, triggered when the reading operation is
    // completed (whether success or failure)
    fReader.onloadend = function() {
      let wrap = document.createElement('div');
      wrap.classList.add("file-label", "file-label--active", "col-2");
      let img = document.createElement('img');
      img.classList.add("frame-img");
      // set the img src attribute to the file's contents (from read operation)
      img.src = fReader.result;
      let imgCapt = document.createElement('p');
      // the name prop of the file contains the file name, and the size prop
      // the file size. convert bytes to KB for the file size
      let fSize = (file.size / 1000) + ' KB';
      imgCapt.innerHTML = `<span class="fName">${file.name}</span><span class="fSize">${fSize}</span><span class="fType">${file.type}</span>`;
      gallery.appendChild(wrap).appendChild(img);
      gallery.appendChild(wrap).appendChild(imgCapt);
    }

    const rmContainer = document.getElementById(`${idPrefix}-rm-container`);
    if (rmContainer && rmContainer.childElementCount === 0) {
      const rmButton = document.createElement("input");
      rmButton.type = "button";
      rmButton.id = `${idPrefix}-rm-button`;
      rmButton.value = `Remove All Images`;
      rmContainer.appendChild(rmButton);
    }

  } else {
    console.error("Only images are allowed!", file);
  }
}

function filesManager(idPrefix, files) {
  // spread the files array from the DataTransfer.files property into a new
  // files array here
  files = [...files];
  // send each element in the array to both the upFile and previewFile
  // functions
  // TODO: instead of upFile, add to a list of files somewhere...
  // files.forEach((file) => upFile(idPrefix, file));
  files.forEach((file) => previewFile(idPrefix, file));
}

// Onload
window.addEventListener("load", (event) => {
  const forms = document.querySelectorAll("form");
  for (const form of forms) {
    const onInput = () => {
      // Handle the different types of forms here.
      if (form) {
        createJsonOutput(form);
      }
    };
    if (form.id === "basic-form" || form.id === "modifiers" || form.id === "skins")
      form.addEventListener("input", onInput);
  }

  setupStartingWeapon();
  createChoices();
  setupDownload();
});



