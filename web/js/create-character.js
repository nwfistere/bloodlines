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

const clone = (obj) => { return JSON.parse(JSON.stringify(obj)); }

const createJsonOutput = (form) => {
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
      // Plus one because the initial character data takes up the first slot.
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
      const skinField = field.id.substring(field.id.indexOf("-") + 1, field.id.lastIndexOf("-"));
      const num = parseInt(field.id.substring(field.id.lastIndexOf("-") + 1, field.id.length));

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
      return field.value.split(/(\\|\/)/g).pop();
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

const createSkinFormPairDiv = () => {
  const div = document.createElement("div");
  div.setAttribute("class", "form-pair skin-form-pair");
  return div;
};

const createInput = (type, name, id) => {
  const input = document.createElement("input");
  input.setAttribute("type", type);
  input.setAttribute("name", name);
  input.setAttribute("id", id);
  return input;
}

const createLabel = (innerHTML) => {
  const label = document.createElement("label");
  label.innerHTML = innerHTML;
  return label;
}

let numOfSkins = 0;
let numOfModifiers = 0;

const createSkinElement = (labelName, inputId, inputType, required = false, readOnly = false, value = undefined) => {
  const label = createLabel(labelName);
  label.setAttribute("for", inputId);
  const input = createInput(inputType, inputId, inputId);
  input.required = required;
  input.readOnly = readOnly;
  if (value !== undefined) {
    input.value = value;
  }
  const div = createSkinFormPairDiv();
  div.appendChild(label);
  div.appendChild(input);
  return div;
}

document.getElementById("addSkinForm").addEventListener("click", () => {
  const div = document.createElement("div");
  div.setAttribute("class", "skin-form-instance");
  div.setAttribute("id", "skin-form-" + numOfSkins);

  div.appendChild(createLabel("Skin " + (numOfSkins + 1)));
  div.appendChild(createSkinElement("Id", "skin-id-" + numOfSkins, "number", false, true));
  div.appendChild(createSkinElement("Skin name", "skin-name-" + numOfSkins, "text"));
  div.appendChild(createSkinElement("Texture Name", "skin-textureName-" + numOfSkins, "text", false, true));
  div.appendChild(createSkinElement("Sprite Portrait", "skin-spriteName-" + numOfSkins, "file"));
  div.appendChild(createSkinElement("Walking Frames", "skin-walkingFrames-" + numOfSkins, "number", false, true));
  div.appendChild(createSkinElement("Unlocked", "skin-unlocked-" + numOfSkins, "checkbox"));
  div.appendChild(createSkinElement("Frames", "skin-frames-" + numOfSkins, "file"));

  document.getElementById("skinContainer").appendChild(div);
  document.getElementById("skin-id-" + numOfSkins).value = numOfSkins;
  document.getElementById("skin-textureName-" + numOfSkins).value = "characters";
  document.getElementById("skin-walkingFrames-" + numOfSkins).value = 4;
  document.getElementById("skin-unlocked-" + numOfSkins).checked = true;
  numOfSkins += 1;
  document.getElementById("removeSkinForm").hidden = false;
});

document.getElementById("removeSkinForm").addEventListener("click", () => {
  if (numOfSkins > 0) {
    const element = document.getElementById("skin-form-" + (numOfSkins - 1));
    element.parentNode.removeChild(element);
    numOfSkins -= 1;
    if (numOfSkins === 0) {
      document.getElementById("removeSkinForm").hidden = true;
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
  const div = createSkinFormPairDiv();
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
  document.getElementById("removeModifierForm").hidden = false;
});

document.getElementById("removeModifierForm").addEventListener("click", () => {
  document.getElementById("spriteName").files
  if (numOfModifiers > 0) {
    const element = document.getElementById("modifier-form-" + (numOfModifiers - 1));
    element.parentNode.removeChild(element);
    numOfModifiers -= 1;
    if (numOfModifiers === 0) {
      document.getElementById("removeModifierForm").hidden = true;
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
    // items: equipmentJson.equipment
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
}

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
}


// Onload
window.addEventListener("load", (event) => {
  const forms = document.querySelectorAll("form");
  for (const form of forms) {
    const onInput = () => {
      // Handle the different types of forms here.
      createJsonOutput(form);
    };
    if (form.id === "basic-form" || form.id === "modifiers" || form.id === "skins")
      form.addEventListener("input", onInput);
  }

  setupStartingWeapon();
  createChoices();
  setupDownload();
});

