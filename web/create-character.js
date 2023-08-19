var weaponListJson = {
  "weapons": [ "VOID", "MAGIC_MISSILE", "HOLY_MISSILE", "WHIP", "VAMPIRICA", "AXE", "SCYTHE", "KNIFE", "THOUSAND", "HOLYWATER", "BORA", "DIAMOND", "FIREBALL", "HELLFIRE", "HOLYBOOK", "VESPERS", "CROSS", "HEAVENSWORD", "GARLIC", "VORTEX", "LAUREL", "THORNS", "LIGHTNING", "LOOP", "PENTAGRAM", "ROSARY", "SIRE", "SILF", "SILF2", "SILF3", "BONE", "LANCET", "SONG", "MANNAGGIA", "CHERRY", "CART", "CART2", "GATTI", "STIGRANGATTI", "GATTI_SCRATCH", "GATTI_SCUFFLE", "FLOWER", "GUNS", "GUNS2", "GUNS3", "TRAPANO", "TRAPANO2", "SARABANDE", "FIREEXPLOSION", "NDUJA", "POWER", "AREA", "SPEED", "COOLDOWN", "DURATION", "AMOUNT", "MAXHEALTH", "ARMOR", "MOVESPEED", "MAGNET", "GROWTH", "LUCK", "GREED", "REVIVAL", "SHIELD", "REGEN", "CURSE", "SILVER", "GOLD", "ROCHER", "ROCHEREXP", "LEFT", "RIGHT", "CORRIDOR", "SHROUD", "COLDEXPLOSION", "WINDOW", "PANDORA", "VENTO", "VENTO2", "VENTO2_EXPLO", "VENTO2_EXTRA", "ROBBA", "WINDOW2", "SILF_COUNTER", "SILF2_COUNTER", "RAYEXPLOSION", "CANDYBOX", "GUNS_COUNTER", "GUNS2_COUNTER", "JUBILEE", "VICTORY", "SOLES", "GATTI_COUNTER", "NDUJA_COUNTER", "TRIASSO1", "TRIASSO2", "TRIASSO3", "BLOODLINE", "CANDYBOX2", "BUBBLES", "ASTRONOMIA", "BLOOD_GARLIC", "BLOOD_SONG", "BLOOD_PENTAGRAM", "BLOOD_LANCET", "BLOOD_LAUREL", "JUBILEE_RAYS", "MISSPELL", "MISSPELL2", "SILVERWIND", "SILVERWIND2", "FOURSEASONS", "FOURSEASONS2", "SUMMONNIGHT", "SUMMONNIGHT2", "MIRAGEROBE", "MIRAGEROBE2", "BUBBLES2", "NIGHTSWORD", "NIGHTSWORD2", "BOCCE", "BOCCE_COUNTER", "SPELL_STRING", "SPELL_STREAM", "SPELL_STRIKE", "SPELL_STROM", "BONE2", "BONE_ARM", "TETRAFORCE", "SHADOWSERVANT", "SHADOWSERVANT2", "PRISMATICMISS", "PRISMATICMISS2", "FLASHARROW", "FLASHARROW2", "SEC_MILLIONAIRE", "SWORD", "SWORD2", "PARTY", "SWORD_FINISHER", "LEGIONNAIRE", "PHASER", "SHADOWSERVANT_COUNTER", "PARTY_COUNTER", "ACADEMYBADGE", "INSATIABLE", "CHERRY2", "CHERRY_STAR_EXPLO", "CHERRY_STAR" ]
}

var weaponsSelect = document.getElementById("weaponsSelect");

for (var i = 0; i < weaponListJson.weapons.length; i++) {
  var option = document.createElement("option");
  option.text = weaponListJson.weapons[i];
  option.value = weaponListJson.weapons[i];
  weaponsSelect.add(option);
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

const createJsonOutput = () => {
  const output = document.getElementById("outputJsonTextArea");
  const json = clone(characterJson);
  const char = json.character[0];
  // char. = document.getElementById("");
  char.charName = document.getElementById("firstName").value;
  char.surname = document.getElementById("lastName").value;
  char.startingWeapon = document.getElementById("weaponsSelect").value;
  char.cooldown = parseInt(document.getElementById("cooldown").value) || 0;
  char.spriteName = document.getElementById("sprite").value.split(/(\\|\/)/g).pop();

  for (var i = 0; i < numOfSkins; i++) {
    const skin = clone(skinJson);
    skin.id = i;
    
    skin.name =  document.getElementById("skinName" + i).value;
    skin.textureName = "Default";
    skin.spriteName = document.getElementById("skinSprite" + i).value.split(/(\\|\/)/g).pop();
    skin.walkingFrames =  parseInt(document.getElementById("walkingFrames" + i).value) || 0;
    skin.unlocked =  document.getElementById("unlocked" + i).checked;

    char.skins[i] = skin;
  }

  char.description = document.getElementById("description").value;
  char.isBought = document.getElementById("isBought").checked;
  char.price = parseInt(document.getElementById("price").value) || 0;

  char.bodyOffset = {
    x: parseInt(document.getElementById("bodyOffsetX").value) || 0,
    y: parseInt(document.getElementById("bodyOffsetY").value) || 0
  };

  json.character[0] = char;
  setJsonOutputValue(json);
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

document.getElementById("addSkinForm").addEventListener("click", () => {
  const div = document.createElement("div");
  div.setAttribute("class", "skin-form-instance");
  div.setAttribute("id", "skin-form-" + numOfSkins);

  const nameLabel = createLabel("Skin name");
  nameLabel.setAttribute("for", "skinName" + numOfSkins);
  const nameText = createInput("text", "skinName" + numOfSkins, "skinName" + numOfSkins);
  const nameDiv = createSkinFormPairDiv();
  nameDiv.appendChild(nameLabel);
  nameDiv.appendChild(nameText);

  const spriteLabel = createLabel("Sprite");
  spriteLabel.setAttribute("for", "skinSprite" + numOfSkins);
  const spriteFile = createInput("file", "skinSprite"  + numOfSkins, "skinSprite"  + numOfSkins);
  const spriteDiv = createSkinFormPairDiv();
  spriteDiv.appendChild(spriteLabel);
  spriteDiv.appendChild(spriteFile);

  const framesLabel = createLabel("Walking Frames (Coming soon)");
  framesLabel.setAttribute("for", "walkingFrames" + numOfSkins);
  const framesNum = createInput("number", "walkingFrames" + numOfSkins, "walkingFrames" + numOfSkins);
  framesNum.readOnly = true;
  const framesDiv = createSkinFormPairDiv();
  framesDiv.appendChild(framesLabel);
  framesDiv.appendChild(framesNum);

  const unlockedLabel = createLabel("Unlocked");
  unlockedLabel.setAttribute("for", "unlocked" + numOfSkins);
  const unlocked = createInput("checkbox", "unlocked" + numOfSkins, "unlocked" + numOfSkins);
  const unlockedDiv = createSkinFormPairDiv();
  unlockedDiv.appendChild(unlockedLabel);
  unlockedDiv.appendChild(unlocked);

  const divLabel = document.createElement("label");
  divLabel.innerHTML = "Skin " + (numOfSkins + 1);

  div.appendChild(divLabel);
  div.appendChild(nameDiv);
  div.appendChild(spriteDiv);
  div.appendChild(framesDiv);
  div.appendChild(unlockedDiv);

  document.getElementById("skinContainer").appendChild(div);
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


const createModifierElements = (labelName, inputId) => {
  const label = createLabel(labelName);
  label.setAttribute("for", inputId);
  const input = createInput("number", inputId, inputId);
  const div = createSkinFormPairDiv();
  div.appendChild(label);
  div.appendChild(input);
  return div;
}

const createModiferSection = (i) => {
  const div = document.createElement("div");
  div.setAttribute("class", "modifier-form-instance");
  div.setAttribute("id", "modifier-form-" + i);

  div.appendChild(createModifierElements("Max HP", "maxHp" + i));
  div.appendChild(createModifierElements("Armor", "armor" + i));
  div.appendChild(createModifierElements("Regen", "regen" + i));
  div.appendChild(createModifierElements("Move speed", "moveSpeed" + i));
  div.appendChild(createModifierElements("Power", "power" + i));
  div.appendChild(createModifierElements("Area", "area" + i));
  div.appendChild(createModifierElements("Attack? Speed", "speed" + i));
  div.appendChild(createModifierElements("Duration", "duration" + i));
  div.appendChild(createModifierElements("Amount", "amount" + i));
  div.appendChild(createModifierElements("Luck", "luck" + i));
  div.appendChild(createModifierElements("Growth", "growth" + i));
  div.appendChild(createModifierElements("Greed", "greed" + i));
  div.appendChild(createModifierElements("Curse", "curse" + i));
  div.appendChild(createModifierElements("Magnet", "magnet" + i));
  div.appendChild(createModifierElements("Revivals", "revivals" + i));
  div.appendChild(createModifierElements("Rerolls", "rerolls" + i));
  div.appendChild(createModifierElements("Skips", "skips" + i));
  div.appendChild(createModifierElements("Banish", "banish" + i));

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
  if (numOfSkins > 0) {
    const element = document.getElementById("modifier-form-" + (numOfSkins - 1));
    element.parentNode.removeChild(element);
    numOfSkins -= 1;
    if (numOfSkins === 0) {
      document.getElementById("removeModifierForm").hidden = true;
    }
    return false;
  }
  return true;
});

// Onload
window.addEventListener("load", (event) => {
  const forms = document.querySelectorAll("form");
  for (const form of forms) {
    form.addEventListener("input", createJsonOutput);
  }
});