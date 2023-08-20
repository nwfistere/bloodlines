var weaponListJson = {
  "weapons": [ "VOID", "MAGIC_MISSILE", "HOLY_MISSILE", "WHIP", "VAMPIRICA", "AXE", "SCYTHE", "KNIFE", "THOUSAND", "HOLYWATER", "BORA", "DIAMOND", "FIREBALL", "HELLFIRE", "HOLYBOOK", "VESPERS", "CROSS", "HEAVENSWORD", "GARLIC", "VORTEX", "LAUREL", "THORNS", "LIGHTNING", "LOOP", "PENTAGRAM", "ROSARY", "SIRE", "SILF", "SILF2", "SILF3", "BONE", "LANCET", "SONG", "MANNAGGIA", "CHERRY", "CART", "CART2", "GATTI", "STIGRANGATTI", "GATTI_SCRATCH", "GATTI_SCUFFLE", "FLOWER", "GUNS", "GUNS2", "GUNS3", "TRAPANO", "TRAPANO2", "SARABANDE", "FIREEXPLOSION", "NDUJA", "POWER", "AREA", "SPEED", "COOLDOWN", "DURATION", "AMOUNT", "MAXHEALTH", "ARMOR", "MOVESPEED", "MAGNET", "GROWTH", "LUCK", "GREED", "REVIVAL", "SHIELD", "REGEN", "CURSE", "SILVER", "GOLD", "ROCHER", "ROCHEREXP", "LEFT", "RIGHT", "CORRIDOR", "SHROUD", "COLDEXPLOSION", "WINDOW", "PANDORA", "VENTO", "VENTO2", "VENTO2_EXPLO", "VENTO2_EXTRA", "ROBBA", "WINDOW2", "SILF_COUNTER", "SILF2_COUNTER", "RAYEXPLOSION", "CANDYBOX", "GUNS_COUNTER", "GUNS2_COUNTER", "JUBILEE", "VICTORY", "SOLES", "GATTI_COUNTER", "NDUJA_COUNTER", "TRIASSO1", "TRIASSO2", "TRIASSO3", "BLOODLINE", "CANDYBOX2", "BUBBLES", "ASTRONOMIA", "BLOOD_GARLIC", "BLOOD_SONG", "BLOOD_PENTAGRAM", "BLOOD_LANCET", "BLOOD_LAUREL", "JUBILEE_RAYS", "MISSPELL", "MISSPELL2", "SILVERWIND", "SILVERWIND2", "FOURSEASONS", "FOURSEASONS2", "SUMMONNIGHT", "SUMMONNIGHT2", "MIRAGEROBE", "MIRAGEROBE2", "BUBBLES2", "NIGHTSWORD", "NIGHTSWORD2", "BOCCE", "BOCCE_COUNTER", "SPELL_STRING", "SPELL_STREAM", "SPELL_STRIKE", "SPELL_STROM", "BONE2", "BONE_ARM", "TETRAFORCE", "SHADOWSERVANT", "SHADOWSERVANT2", "PRISMATICMISS", "PRISMATICMISS2", "FLASHARROW", "FLASHARROW2", "SEC_MILLIONAIRE", "SWORD", "SWORD2", "PARTY", "SWORD_FINISHER", "LEGIONNAIRE", "PHASER", "SHADOWSERVANT_COUNTER", "PARTY_COUNTER", "ACADEMYBADGE", "INSATIABLE", "CHERRY2", "CHERRY_STAR_EXPLO", "CHERRY_STAR" ]
}

var startingWeapon = document.getElementById("startingWeapon");

for (var i = 0; i < weaponListJson.weapons.length; i++) {
  var option = document.createElement("option");
  option.text = weaponListJson.weapons[i];
  option.value = weaponListJson.weapons[i];
  startingWeapon.add(option);
}

const handleSubmit = (event) => {
  console.log("Submit handled!");
};

const characterJson = {
  "version": "0.1",
  "character": [
    {
      "level": 1,
      "startingWeapon": "",
      "cooldown": 1,
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
      "maxHp": 100,
      "armor": 1,
      "regen": 0,
      "moveSpeed": 0,
      "power": 0,
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
      "showcase": [],
      "hiddenWeapons": [],
      "bodyOffset": {
        x: 0,
        y: 0
      }
    },
  ]
};

const skinJson = {
    "id": 0,
    "name": "Default",
    "textureName": "characters",
    "spriteName": "",
    "walkingFrames": 1,
    "unlocked": true
};

const clone = (obj) => { return JSON.parse(JSON.stringify(obj)); }

const createJsonOutput2 = (form) => {
  const currentJson = document.getElementById("outputJsonTextArea");
  let json = {};

  if (currentJson == null || currentJson.textContent.length == 0) {
    json = clone(characterJson);
  } else {
    json = JSON.parse(currentJson.textContent);
  }

  const char = json.character[0];
  for (let field of form.elements) {
    if (!field.id || field.disabled || field.type === 'reset' || field.type === 'submit' || field.type === 'button') {
      continue;
    }

    let fieldId = field.id;

    if (field.id.endsWith("-initial")) {
      // Remove mod- and -initial
      fieldId = fieldId.substring(4, fieldId.length - 8);
    } else if (field.id.includes(".")) {
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
      const num = parseInt(field.id.substring(field.id.lastIndexOf("-") + 1, field.id.length)) + 1;

      console.debug("modifier num: " + num);

      while (json.character[num] == null) {
        json.character.push({});
      }

      const modObject = json.character[num];
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
    default:
      console.error("Unknown input type: " + field.type);
  }

  return null;
}

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

const createSkinElement = (labelName, inputId, inputType, required = false, readOnly = false) => {
  const label = createLabel(labelName);
  label.setAttribute("for", inputId);
  const input = createInput(inputType, inputId, inputId);
  input.required = required;
  input.readOnly = readOnly;
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
  div.appendChild(createSkinElement("Sprite", "skin-textureName-" + numOfSkins, "text", false, true));
  div.appendChild(createSkinElement("Sprite", "skin-spriteName-" + numOfSkins, "file"));
  div.appendChild(createSkinElement("Walking Frames", "skin-walkingFrames-" + numOfSkins, "number", false, true));
  div.appendChild(createSkinElement("Unlocked", "skin-unlocked-" + numOfSkins, "checkbox"));

  document.getElementById("skinContainer").appendChild(div);
  document.getElementById("skin-id-" + numOfSkins).value = numOfSkins;
  document.getElementById("skin-textureName-" + numOfSkins).value = "characters";
  document.getElementById("skin-walkingFrames-" + numOfSkins).value = 4;
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
  const label = createLabel(labelName);
  label.setAttribute("for", inputId);
  const input = createInput("number", inputId, inputId);
  input.required = required;
  input.step = step;
  const div = createSkinFormPairDiv();
  div.appendChild(label);
  div.appendChild(input);
  return div;
}

const createModiferSection = (i) => {
  const div = document.createElement("div");
  div.setAttribute("class", "modifier-form-instance");
  div.setAttribute("id", "modifier-form-" + i);

  div.appendChild(createLabel("Stat Modifier " + (i + 1)));
  div.appendChild(createModifierElement("Level", "mod-level-" + i, true, 1));
  div.appendChild(createModifierElement("Max HP", "mod-maxHp-" + i));
  div.appendChild(createModifierElement("Armor", "mod-armor-" + i));
  div.appendChild(createModifierElement("Regen", "mod-regen-" + i));
  div.appendChild(createModifierElement("Move speed", "mod-moveSpeed-" + i));
  div.appendChild(createModifierElement("Power", "mod-power-" + i));
  div.appendChild(createModifierElement("Area", "mod-area-" + i));
  div.appendChild(createModifierElement("Attack? Speed", "mod-speed-" + i));
  div.appendChild(createModifierElement("Duration", "mod-duration-" + i));
  div.appendChild(createModifierElement("Amount", "mod-amount-" + i));
  div.appendChild(createModifierElement("Luck", "mod-luck-" + i));
  div.appendChild(createModifierElement("Growth", "mod-growth-" + i));
  div.appendChild(createModifierElement("Greed", "mod-greed-" + i));
  div.appendChild(createModifierElement("Curse", "mod-curse-" + i));
  div.appendChild(createModifierElement("Magnet", "mod-magnet-" + i));
  div.appendChild(createModifierElement("Revivals", "mod-revivals-" + i));
  div.appendChild(createModifierElement("Rerolls", "mod-rerolls-" + i));
  div.appendChild(createModifierElement("Skips", "mod-skips-" + i));
  div.appendChild(createModifierElement("Banish", "mod-banish-" + i));

  return div;
}

document.getElementById("addModifierForm").addEventListener("click", () => {
  let i = numOfModifiers;
  const div = createModiferSection(i);
  document.getElementById("modifierContainer").appendChild(div);
  numOfModifiers += 1;
  document.getElementById("removeModifierForm").hidden = false;
});

document.getElementById("removeModifierForm").addEventListener("click", () => {
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

let initialModifierSpawned = false;

document.getElementById("addInitialModifierForm").addEventListener("click", () => {
  const div = createModiferSection("initial");
  document.getElementById("initialModifierContainer").appendChild(div);
  initialModifierSpawned = true;
  document.getElementById("removeInitialModifierForm").hidden = false;
  document.getElementById("addInitialModifierForm").hidden = true;
  const level = document.getElementById("mod-level-initial");
  level.value = 1;
  level.readOnly = true;

  const parentForm = document.querySelector("#basic");
  const onInput = () => {
    // Handle the different types of forms here.
    createJsonOutput2(parentForm);
  };

  parentForm.addEventListener("input", onInput);
});

document.getElementById("removeInitialModifierForm").addEventListener("click", () => {
  if (initialModifierSpawned) {
    const element = document.getElementById("modifier-form-initial");
    element.parentNode.removeChild(element);
    initialModifierSpawned = false;
    document.getElementById("removeInitialModifierForm").hidden = true;
    document.getElementById("addInitialModifierForm").hidden = false;
    return false;
  }
  return true;
});


// Onload
window.addEventListener("load", (event) => {
  const forms= document.querySelectorAll("form");
  for (const form of forms) {
    const onInput = () => {
      // Handle the different types of forms here.
      createJsonOutput2(form);
    };
    if (form.id === "basic" || form.id === "modifiers" || form.id === "skins")
      form.addEventListener("input", onInput);
  }
});