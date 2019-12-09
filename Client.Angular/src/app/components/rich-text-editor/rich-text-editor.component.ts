import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import Quill from 'quill';

@Component({
  selector: 'app-rich-text-editor',
  templateUrl: './rich-text-editor.component.html',
  styleUrls: ['./rich-text-editor.component.css']
})
export class RichTextEditorComponent implements OnInit {
  quillEditor: Quill;
  @Output() emitContent = new EventEmitter();

  constructor() { }

  ngOnInit() {
    this.quillEditor = new Quill('#editor', {
      modules: {
        toolbar: '#toolbar',
      },
      theme: 'snow'
    });
  }

  emitContentButton() {
    this.emitContent.emit(this.quillEditor.getContents());
    this.quillEditor.deleteText(0, this.quillEditor.getLength());
  }
}
