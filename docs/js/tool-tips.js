// https://stackoverflow.com/a/69340293/6844405


window.addEventListener("load", (event) => {
  const elements = [...document.querySelectorAll('[tip]')]

  for (const el of elements) {
    const tip = document.createElement('div')
    tip.classList.add('tooltip')
    tip.textContent = el.getAttribute('tip')
    const x = el.hasAttribute('tip-left') ? 'calc(-100% - 5px)' : '16px'
    const y = el.hasAttribute('tip-top') ? '-100%' : '0'
    tip.style.transform = `translate(${x}, ${y})`
    el.appendChild(tip)
    el.onpointermove = e => {
      if (e.target !== e.currentTarget) return
      
      const rect = tip.getBoundingClientRect()
      const rectWidth = rect.width + 16
      const vWidth = window.innerWidth - rectWidth
      const rectX = el.hasAttribute('tip-left') ? e.clientX - rectWidth: e.clientX + rectWidth    
      const minX = el.hasAttribute('tip-left') ? 0 : rectX 
      const maxX = el.hasAttribute('tip-left') ? vWidth : window.innerWidth 
      const x = rectX < minX ? rectWidth : rectX > maxX ? vWidth : e.clientX
      tip.style.left = `${x}px`
      tip.style.top = `${e.clientY}px`
    }
  }
});
