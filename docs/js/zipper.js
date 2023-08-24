// Get's the fields and zips them!
class FieldsToZipFile {
  /**
   * 
   * @param {string} jsonFieldId - json text area id
   * @param {[string]} imageFieldIds - Known existing image field ids. May or may not have anything in them.
   * @param {[string]} imageFieldIdPrefixes - Prefixes of ids that may or may not exist. Append integers onto them and search.
   * @param {[File]} fileList - A list of arbitrary files
   */
  constructor(filename, jsonFieldId, imageFieldIds, imageFieldIdPrefixes, fileList) {
    this.filename = filename;
    this.jsonField = document.getElementById(jsonFieldId);
    this.imageFields = [];
    this.OtherFiles = [...fileList];
    
    imageFieldIds.forEach((id) => this.imageFields.push(document.getElementById(id)));

    for (const idPrefix of imageFieldIdPrefixes) {
      let i = 0;
      while (i++ > -1) {
        let field;
        if (null !== (field = document.getElementById(idPrefix + i))) {
          this.imageFields.push(field);
        } else {
          break;
        }
      }
    }
  }

  /**
   * Zips files together and returns the url to download it.
   * @returns string BlobUrl - the url to the zip file.
   */
  async zip() {
    const files = [...this.OtherFiles];
    files.push(new File([this.jsonField.textContent], "character.json", {
      type: "text/json",
    }));

    for (const field of this.imageFields) {
      if (field.files) {
        for (const file of field.files) {
          files.push(file);
        }
      }
    }

    const zipper = new Zipper(this.filename, files);

    return zipper.getBlobUrl();
  }
};

class Zipper {
  static zipWriter;
  constructor(zipname, files) {
    this.zipname = zipname;
    this.files = files;
    Zipper.zipWriter = new zip.ZipWriter(new zip.BlobWriter("application/zip"), { bufferedWrite: true, useCompressionStream: false });
    for (const file of this.files) {
      Zipper.zipWriter.add(file.name, new zip.BlobReader(file), {});
    }
  }

  async getBlobUrl() {
    if (Zipper.zipWriter) {
      const blobURL = URL.createObjectURL(await Zipper.zipWriter.close());

      const anchor = document.createElement("a");
      const clickEvent = new MouseEvent("click");
      anchor.href = blobURL;
      anchor.download = this.zipname;
      anchor.dispatchEvent(clickEvent);
    }
  }
};
